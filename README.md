# docsearch

Når i skal køre løsningen skal i have 4 terminalvinduer åbne. Først, i 2 separate vinduer, navigerer i ind i SearchAPI folder'en. Så kører i;
clear && dotnet run --launch-profile https
i det ene vinduer, og;
clear && dotnet run --launch-profile https2
i det andet.
Herefter navigerer i, i det 3. vindue, ind i LoadBalancer folder, og kører
clear && dotnet run
Herefter kan i, i det 4. vindue, navigere ind i ConsoleSearch og køre;
clear && dotnet run
Søgemaskinen er nu aktiv.

Filer i skal finde og ændre path til noget lign på egen computer:
SearchProxy.cs - ligger i Shared folder
ændr connectionString til at lede til hvorend i ligger jeres users.db SQLite fil

Paths.cs - ligger i Shared folder
ændr FOLDER til at pege til jeres seData mappe
ændr de to paths neders til at pege på jeres searchDBm1+2.db filer