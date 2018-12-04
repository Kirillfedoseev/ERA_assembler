# ERA Assembler project
This project is The ERA assembler code hand-written compiler iplemented using C# and .Net Tests.

## Contributors
Kirill Fedoseev - Tranlator and Mapper, Leading
Elena Lebedeva - Lexer logic
Valeria Ahmetzhanova - Lexer tests
Victoria Rotaru - Translator tests
Anastasia Kirdyasheva - Mapper tests


## How to launch
Open the `ERA_Assembly.sln` in VS.

If you want to get generated binary code, then run the Executer (by default in VS you should only push Start button).
Also, before it, you can change the input file `in.txt`.
The machine code will be in `out.txt` after compilation.

If you want to test the application, then run tests from VS, you do it by pushing button run all tests in menu or combination `ctrl+R+A`

## How it works

    Assembler code -> Lexer -> Tokens -> Translator <-> Mapper
                                           ||
                                           \/
                                      Machine code

Assembler code divide onto tokens, after it goes to translator, which convert it to commands, <br>
after commands and labels data goes to mapper and labels are mapped onto memory addresses.

## Additional materials 
All commands, examples of code and grammer you can find in that doc `Project_ERA.pdf`.
