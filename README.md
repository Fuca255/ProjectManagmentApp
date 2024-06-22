# Neophodni alati i paketi za pokretanje aplikacije
1. Instalacija Angular-a
     - Proverite da li imate instaliranu podržanu verziju Node.js za Angular:
        - Pokrenite sledeću komandu iz terminala: 'node --version'.
        - Potvrdite da li prikazana verzija ispunjava zahteve Angulara.
        - Ukoliko nemate instaliranu verziju node.js, može se skinuti na [sajt] (https://nodejs.org/en/download) .
     - Kada ste instalirali node.js i npm, sledeći korak je instalacija Angular CLI koji pruža alate za efikasan Angular razvoj.
        - Iz terminala pokrenite sledeću komandu 'npm install -g @angular/cli'.
     - Angular je sada instaliran na vašem računara i možete koristiti bilo koji alat za pravljenje aplikacija pomoću Angular'a. Mi smo koristili Visual Studio Code pa njega i preporučujemo.

2. .NET SDK 8 - potrebno je za razvoj i pokretanje bekenda. Treba otici na zvaničan .NET [sajt](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), gde možemo da skinemo .NET okruženje za operativni sistem koji želimo. Nakon instalacije treba instalirati ovo okruženje po uputstvima u instalaciji.
3. Instalacija Visual studio code
    - Instalirati visual studio code sa zvaničnog sajta.
    - Intalirati ektenzije
        + C# Dev Kit
        + C# Extensions
        + NuGet Gallery
4. Potrebni paketi nakon instaliranja .NET SDK 8 za pokretanje (instalirati preko Nuget Gallerz ekstenzije u VS kodu, ako ne postoje)
    - Microsoft.AspNetCore.Authentication.JwtBearer
    - Microsoft.EntityFrameworkCore.Design
    - Microsoft.EntityFrameworkCore.Tools
    - Microsoft.EntityFrameworkCore.Sqlite


# Pokretanje projekta
1. Kreiranje baze podataka se radi tako što:
    - Otvori se terminal na računaru ili u editoru (preporuka VS code)
    - Postaviti putanju na terminala na folder server
    - Zatim pokrenuti komandu *dotnet ef migrations add ime_migracije*
    - Nakon toga treba pokrenuti komandu *dotnet ef database update*
    - Nakon ove komande baza će biti kreirana

2. Pokretanje bekenda
    - Otvori se terminal na računaru ili u editoru (preporuka VS code)
    - Postaviti putanju na terminala na folder server
    - Pokrenuti komandu *dotnet watch run*, može da se pokrene i komanda *dotnet watch run seed* koja će uneti već neke default podatke u bazu.
    - Nakon ovoga trebalo bi da se otvori Swagger preko kojeg možemo da komuniciramo sa bazom i on bi trebao da se nalazi na adresi http://localhost:5295

3. Pokretanje frontenda
    - Otvori se terminal na računaru ili u editoru (preporuka VS code)
    - Postaviti putanju na terminala na folder client
    - Zatim pokrenuti komandu *npm install* kako bi se instalirali svi potrebni paketi od koje aplikacija zavisi
    - Ukucati komandu *ng serve -o* koja ce pokrenuti frontend aplikacije i otvoriti je u default-nom browseru


# Pokretanje produkcione verzije projekta
Ovo podrazumeva da će se projekat pokretati na nekom serveru a ne kod nas lokalno.
Prilikom pokretanje Angular projekta trebaju nam 2 terminala da bi se on podigao na server. U jednom terminalu umesto *ng serve* komande koristi *ng build --watch* i ovde će moći da se prati kako bi se aplikacija ponašala da je na serveru. Na drugom terminalu treba da se pokrene server, npr ako bi koristili lite-server, ukucali bi komandu *lite-server --baseDir="dist/project-name"*. Ovo je bi bio način podizanja aplikacije sa našeg diska. 

Naša aplikacija se nalazi na sledećoj [adresi](http://softeng.pmf.kg.ac.rs:10190/)

# Korisnicki nalozi za testiranje:

1. Administrator
    - username:
        + ethanmoore
    - password:
        + password707

2. Project Manager
    - username:
        + aleksandra_stanic
    - password:
        + Boki037

3. Project Worker
    - username:
        + alexjohnson
    - password:
        + password789

4. User
    - username:
        + jessicalee
    - password:
        + password404

5. Guest
    - username:
        + oliviaanderson
    - password:
        + password606

# Tim
1. Aleksandra Stanić
2. Anastasija Iličić
3. Aleksa Kosovac
4. Damjan Pavlović
5. Stefan Vučićević