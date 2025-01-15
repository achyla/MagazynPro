# Dokumentacja Systemu MagazynPro

## Wprowadzenie

MagazynPro to zaawansowany system zarządzania magazynem, który wykorzystuje najnowsze technologie, takie jak ASP.NET Core MVC i SQL Server, do skutecznego zarządzania produktami, zamówieniami i danymi magazynowymi. System ten zaprojektowano z myślą o firmach dążących do maksymalizacji efektywności swoich procesów logistycznych. Dzięki intuicyjnemu interfejsowi oraz szerokim możliwościom konfiguracji, MagazynPro może być dopasowany do potrzeb różnych branż i scenariuszy biznesowych.

## Wymagania systemowe

**System operacyjny:**

- Windows 10/11, Linux, macOS.

**Serwer aplikacji:**

- ASP.NET Core w wersji 8.0.10 lub nowszej.

**Serwer bazy danych:**

- Microsoft SQL Server.

**Przeglądarka internetowa:**

- Google Chrome, Firefox, Microsoft Edge.

**Środowisko programistyczne:**

- Visual Studio 2022 z obsługą .NET SDK.

**Dodatkowe wymagania:**

- Stabilne połączenie internetowe dla pobierania aktualizacji.
- Prawa administratora do instalacji oprogramowania i konfiguracji bazy danych.

---

## Instrukcja instalacji

### Konfiguracja połączenia z bazą danych

Aby skonfigurować połączenie z bazą danych, edytuj plik `appsettings.json` w katalogu głównym projektu. Poniżej znajduje się przykładowa konfiguracja:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MagazynProDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

- **Server:** Nazwa lub adres serwera SQL, np. `localhost`.
- **Database:** Nazwa bazy danych, domyślnie `MagazynProDb`.
- **Trusted\_Connection:** Ustaw na `True`, jeśli używasz uwierzytelniania Windows.

### Inicjalizacja bazy danych

1. Otwórz projekt w Visual Studio.
2. Otwórz konsolę menadżera pakietów (Tools > NuGet Package Manager > Package Manager Console).
3. Wprowadź następujące polecenia:
   ```bash
   Add-Migration Initial
   Update-Database
   ```
4. Upewnij się, że baza danych została prawidłowo utworzona i zawiera wszystkie wymagane tabele.

### Uruchamianie aplikacji

1. Upewnij się, że serwer SQL działa poprawnie.
2. W Visual Studio skonfiguruj plik `launchSettings.json`, jeśli konieczne.
3. Uruchom aplikację za pomocą klawisza **F5** lub kliknij przycisk "Start".

---

## Funkcjonalności systemu

### Funkcje użytkownika końcowego

- Przeglądanie listy produktów z magazynu.
- Wyświetlanie szczegółowych informacji o produktach, takich jak cena, dostępność.
- Składanie zamównienia.
- Edycja wcześniej złożonego zamównienia.

### Funkcje administratora systemu

- **Zarządzanie produktami:**

  - Dodawanie nowych produktów do bazy danych.
  - Edytowanie informacji o istniejących produktach, takich jak nazwa, cena i ilość na stanie.
  - Usuwanie produktów.

- **Zarządzanie zamówieniami:**

  - Edytowanie zamówienia klienta.
  - Usuwanie zamówienia klienta.

---

## Obsługa aplikacji

1. **Logowanie i rejestracja:**
   - Zarówno użytkownicy końcowi, jak i administratorzy mogą zakładać konta w systemie.
   - Proces logowania wykorzystuje bezpieczne mechanizmy uwierzytelniania.
2. **Nawigacja po aplikacji:**
   - Strona główna umożliwia przeglądanie zasobów magazynowych.
   - Panel administratora dostępny jest po zalogowaniu się na konto z odpowiednimi uprawnieniami.

---

## Bezpieczeństwo danych

- Wszystkie hasła użytkowników są przechowywane w postaci zaszyfrowanej (haszowanie z użyciem algorytmów takich jak BCrypt), co chroni przed ich wyciekiem.
- Dostęp do danych jest kontrolowany za pomocą ról użytkowników i szczegółowo zdefiniowanych uprawnień.
- Regularne kopie zapasowe bazy danych są zalecane, aby zminimalizować ryzyko utraty danych w przypadku awarii.

---

## Zalecenia dla użytkowników

- **Aktualizacje:** Upewnij się, że używasz najnowszej wersji systemu, aby korzystać z nowych funkcji i poprawek bezpieczeństwa.
- **Optymalizacja:** Regularnie sprawdzaj stan magazynu.
- **Szkolenia:** Zaleca się przeszkolenie personelu w zakresie korzystania z aplikacji w celu zwiększenia efektywności.

---

