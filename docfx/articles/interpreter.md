# The Interpreter

**Lapis.Script.Execution** provides basic infrastructure for an intepreter, 
including the syntax trees and runtime contexts.

Below are example scripts reflecting the features of the script language.

### Flow Control
Condition statements:
```typescript
var x = 0
if (x == 1)
    x++
else
    x--
print(x)

x = 0
switch (x) {
	case 0 : print(0); break
	case 1 : print(1); break
	default : print("default")
}
```

Looping statements:
```typescript
for (var i = 0; i < 5; i++) {
    print(i)
}

while (i > 0)
    print(i--)

do {
    print(i++)
} while (i < 5)
```

### Function
Recursion:
```typescript
function factorial(x) {
    if(x == 0)
        return 1   
    else    
        return x * factorial(x - 1)
}
print(factorial(5)); // 120
```

Closure:
```typescript
var x = 0
function clousure() {
    print(x++)    
}
clousure() // 0
clousure() // 1
```

A lambda expression:
```typescript
print((x => -x)(2)) // -2
```

### Complex Object
An array:
```typescript
var array = [ 0, 1, 2 ]
print(array[1])
```

Array decomposition:
```typescript
var a = 0; 
var b = 0; 
var c = 0;
[a, [b, c]] = [ 1, [2, 3]]
print(a, b, c) // 1 2 3
```

An object:
```typescript
var dict = { x : 0, y : 1 }
print(dict.x)
```

### Class
A simple class with a field, a constructor, and a method:
```typescript
class Greeter {
    greeting;

    constructor(message) {
        this.greeting = message;
    }

    greet() {
        return "Hello, " + this.greeting;
    }
}

var greeter = new Greeter("world");
print(greeter.greet()); // Hello, world
```

### Property and Indexer
A class with property accessors:
```typescript
class Person {
    private m_name = "N/A";
    private m_Age = 0;

    public Name {
        get { return this.m_name; }
        set { this.m_name = value; }
    }
    public Age {
        get { return this.m_Age; }
        set { this.m_Age = value; }
    }

    public ToString() {
        return "Name = " + this.Name + ", Age = " + this.Age;
    }
}

var person = new Person();
print("Person details - " + person.ToString());
// Person details - Name = N/A, Age = 0

person.Name = "Joe";
person.Age = 99;
print("Person details - " + person.ToString());
// Person details - Name = Joe, Age = 99

person.Age += 1;
print("Person details - " + person.ToString());
// Person details - Name = Joe, Age = 100
```

A class with an indexer:
```typescript
class DayCollection {
    private days = [ "Sun", "Mon", "Tues", "Wed", "Thurs", "Fri", "Sat" ];

    private GetDay(testDay) {
        for (var i = 0; i < this.days.length; i++) {
            if (this.days[i] == testDay) {
                return i;
            }       
        }
        return -1;
    }

    public this[day] {
        get { return (this.GetDay(day)); }
    }
}

var week = new DayCollection();
print(week["Fri"]); // 5
print(week["Made-up Day"]); // -1
```

### Event
The publisher-subscriber pattern:
```typescript
class EventArgs {
    constructor(s) { this.message = s; }    

    public Message {
        get { return this.message; }   
		set { this.message = value; }     
    }

    private message;
}

class Publisher {
    public Name { get; set; }

	constructor(name) { this.Name = name; }

    public RaiseEvent;

    public DoSomething() {      
		this.OnRaiseEvent(new EventArgs("Did something"));
    }

    protected OnRaiseEvent(e) {        
	    var handler = this.RaiseEvent;
        if (handler != null) {            
            e.Message += " by " + this.Name;
            handler(this, e);
        }
    }
}

class Subscriber {
    private id;

    constructor(id, pub) {
        this.id = id;        
        pub.RaiseEvent = this.HandleCustomEvent;
    }

    HandleCustomEvent(sender, e) {
        print(this.id + " received this message: " + e.Message);
    }
}

var pub = new Publisher("pub");
var sub1 = new Subscriber("sub1", pub);
var sub2 = new Subscriber("sub2", pub);
pub.DoSomething();
// sub2 received this message: Did something by pub
```

### Inheritance
Class inheritance and method overriding:
```typescript
class Animal {
    name;

    constructor(name) { this.name = name; }
	
    move(meters) {
        print(this.name + " moved " + meters + "m.");
    }
}

class Snake extends Animal {
    constructor(name) { super(name); }

    move() {
        print("Slithering...");
        super.move(5);
    }
}

class Horse extends Animal {
    constructor(name) { super(name); }
    
    move() {
        print("Galloping...");
        super.move(45);
    }
}

var sam = new Snake("Sammy the Python");
var tom = new Horse("Tommy the Palomino");
sam.move();
// Slithering...
// Sammy the Python moved 5m.
tom.move();
// Galloping...
// Tommy the Palomino moved 45m.
```