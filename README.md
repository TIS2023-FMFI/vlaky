# CEVA manazment nakladania a expedicie vlakov
Projekt TIS 2023 o nakladaní vozňov vlaku

## Predpoklady
Releas z https://github.com/TIS2023-FMFI/vlaky alebo 
naklonovat repozitar a zkompilovat podla https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/?view=aspnetcore-8.0&source=recommendations#publish-to-a-folder

Dalsie:
- PostgreSQL
- Apache
- Aspnetcore-runtime-8.0

## Setup
Tento navod je urceny na sprevadskovanie na OS Windows, kde aplikacia bude spustena ako Windows Service (ine sposoby hostovania tu: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/?view=aspnetcore-8.0) za predpokladu ze proxy server je nastaveny (adresa a port sa daju nastavit v appsettings.json v "Kestrel" pod "Url")

### Databaza
- sprevadckovat PostgreSQL (https://www.postgresqltutorial.com/postgresql-getting-started/install-postgresql-linux/)
- vytvorit databazu (prednastaveny nazov databazi je "fvl-radenie-vlakov")
- vykonat tranzakciu na vytvorenie tabuliek (https://github.com/TIS2023-FMFI/vlaky/blob/main/docs/databaza/DB_CREATE.txt)

### Service setup
- Vytvorit uzivatela pod ktorym aplikacia bu de spustena a nastavit mu prava (https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-8.0&tabs=visual-studio#service-user-account)
- Vytvorenie samotneho Windows service pre aplikaciu (https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-8.0&tabs=visual-studio#create-and-manage-the-windows-service)

## Nastavenia
Vsetky nastavenia sa nachadzaju v subore appsettings.json

### Nastavenie endpoint-ov
V sekcii Kestrel sa da nastavit na akej adrese a porte aplikacia pocuva zmenenim hodnoty v Url, ak sa jedna o localhost adresu tak automaticky nastavy aj 127.0.0.1 s rovnakym portom (toto moze zlihat v pripade ze dany port je pozivany inym service)

### Informacie na pripojenie na Databazu
Nsatavit ako a kam sa aplikacia pripojuje na databazu sa da nastavit upravenim hodnoty DefaultConnection v sekcii ConnectionStrings, ako ich nastavit a dalsie ine nastavenia najdete na https://www.npgsql.org/doc/connection-string-parameters.html

### Nastavenie vystupu logu
Presny formulaciu vypisu do logu sa da upravit v sekcii FileLoggingOptions. Navrhnuta formulacia je rozpisana v docs/Navrh.pdf (https://github.com/TIS2023-FMFI/vlaky/blob/main/docs/N%C3%A1vrh.pdf) v kapitole 3 v sekcii Logova aktivita
