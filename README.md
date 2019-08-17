# Oct-HexToBinaryCompiler

A compiler made for Compiler Design (TI2-314) Class in Unibe. It takes hex and octal numbers and compiles them to their binary equivalent. Can also compile binary operations between the numbers.

## How to run
1. Install dotnet core in development machine
2. Clone Project from Repository
3. Run `dotnet build` command in project folder.
4. Run `dotnet run` command in project folder.

## API

- Internal classes the actually do the compilation process should recieve modifiers Internal Sealed.
- If the main program needs to somehow execute or trigger the classes, and they can't be added to the public phase classes. A new phase class should be added that is to be triggered on main class that actually executes internal process of compiler.
