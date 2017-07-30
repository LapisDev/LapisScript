/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : BlockContext
 * Description : Represents a runtime context in a statement block.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime.ScriptObjects;

namespace Lapis.Script.Execution.Runtime.RuntimeContexts
{
    /// <summary>
    /// Represents a runtime context in a statement block.
    /// </summary>
    public class BlockContext : RuntimeContext
    {
        /// <summary>
        /// Gets the parent scope of the context.
        /// </summary>
        /// <value>The parent scope of the context.</value>
        public override RuntimeContext Scope
        {
            get { return Parent; }
        }

        /// <summary>
        /// Gets the parent scope of the context.
        /// </summary>
        /// <value>The parent scope of the context.</value>
        public RuntimeContext Parent { get; private set; }

        /// <summary>
        /// Gets the type of the block.
        /// </summary>
        /// <value>The type of the block.</value>
        public BlockType BlockType { get; private set; }

        /// <summary>
        /// Gets the class to which the scope belongs.
        /// </summary>
        /// <value>The class of the scope.</value>
        public override IClassObject Class
        {
            get { return Parent.Class; }
        }

        /// <summary>
        /// Gets the <c>this</c> reference bound in the scope.
        /// </summary>
        /// <value>The <c>this</c> reference bound in the scope.</value>
        public override IScriptObject This
        {
            get { return Parent.This; }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="BlockContext"/> class with the specified parameters.
        /// </summary>
        /// <param name="parent">The parent scope of the context.</param>
        /// <param name="blockType">The type of the block.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public BlockContext(
            RuntimeContext parent,
            BlockType blockType)
            : base(CheckNullAndGetMemoryCreator(parent), parent.ObjectCreator, parent.Operators)
        {          
            Parent = parent;
            BlockType = blockType;
        }        

        internal override bool CanBreak
        {
            get
            {
                if (this.BlockType == BlockType.Loop ||
                    this.BlockType == BlockType.Switch)
                    return true;
                else
                    return this.Parent.CanBreak;
            }
        }

        internal override bool CanContinue
        {
            get
            {
                if (this.BlockType == BlockType.Loop)
                    return true;
                else
                    return this.Parent.CanBreak;
            }
        }

        internal override bool CanGoto(string label)
        {
            if (base.CanGoto(label))
                return true;
            else
                return Parent.CanGoto(label);
        }

        internal override bool CanReturn(bool hasValue)
        {
            return Parent.CanReturn(hasValue);
        }      
    }

    /// <summary>
    /// Represents the type of the block context.
    /// </summary>
    public enum BlockType
    {
        /// <summary>
        /// Represents a normal block.
        /// </summary>
        Block,
        /// <summary>
        /// Represents a <c>if</c> block.
        /// </summary>
        If,
        /// <summary>
        /// Represents a <c>while</c>, <c>do-while</c> or <c>for</c> block.
        /// </summary>
        Loop,
        /// <summary>
        /// Represents a  <c>switch</c> block.
        /// </summary>
        Switch
    }
}
