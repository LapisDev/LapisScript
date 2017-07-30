/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IndexerGetter  IndexerSetter
 * Description : Represents a get or set accessor of an indexer.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.OOP
{

    /// <summary>
    /// Represents a <c>get</c> accessor of an indexer.
    /// </summary>
    public class IndexerGetter : ClassMember
    {
        /// <summary>
        /// Gets the method that the accessor wraps.
        /// </summary>
        /// <value>The method that the accessor wraps.</value>
        public ClassMethod Method { get; private set; }

        /// <summary>
        /// Invokes the <c>get</c> accessor of the indexer with the specified parameters and returns the value.
        /// </summary>
		/// <param name="target">The target object on which the indexer is invoked.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <returns>The return value of the indexer.</returns>
        public IScriptObject GetItem(IScriptObject target, List<IScriptObject> indices)
        {
            return Method.Invoke(target, indices);
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="IndexerGetter"/> class using the specified parameter.
        /// </summary>  
        /// <param name="method">The method that the accessor wraps.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public IndexerGetter(ClassMethod method)
        {
            if (method == null)
                throw new ArgumentNullException();
            Method = method;
        }
    }

    /// <summary>
    /// Represents a <c>set</c> accessor of an indexer.
    /// </summary>
    public class IndexerSetter : ClassMember
    {
        /// <summary>
        /// Gets the method that the accessor wraps.
        /// </summary>
        /// <value>The method that the accessor wraps.</value>
        public ClassMethod Method { get; private set; }

        /// <summary>
        /// Invokes the <c>set</c> accessor of the indexer with the specified parameters and the value to set.
        /// </summary>
		/// <param name="target">The target object on which the indexer is invoked.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <param name="value">The value to set.</param>
        public void SetItem(IScriptObject target, List<IScriptObject> indices, IScriptObject value)
        {
            var args = new List<IScriptObject>(indices);
            args.Insert(0, value);
            Method.Invoke(target, args);
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="IndexerSetter"/> class using the specified parameter.
        /// </summary>  
        /// <param name="method">The method that the accessor wraps.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public IndexerSetter(ClassMethod method)
        {
            if (method == null)
                throw new ArgumentNullException();
            Method = method;
        }
    }
}
