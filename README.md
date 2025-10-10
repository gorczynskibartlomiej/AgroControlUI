**Opis projektu**

AgroControlUI to aplikacja internetowa stworzona w technologii ASP.NET MVC przy użyciu języka C#, pełniąca rolę front-endu dla systemu AgroControlApi.
Projekt został zaprojektowany w taki sposób, aby oddzielić warstwę prezentacji (interfejs użytkownika) od logiki biznesowej i obsługi danych, które znajdują się w niezależnym Web API.
Taki podział pozwala na większą elastyczność i skalowalność systemu. Takie rozwiązanie umożliwia w przyszłości łatwe stworzenie innych klientów, takich jak aplikacja mobilna czy panel administracyjny, korzystających z tego samego interfejsu API.

**Architektura i działanie aplikacji**

Projekt AgroControlUI został zbudowany w oparciu o klasyczny wzorzec Model-View-Controller (MVC), który pozwala na logiczne rozdzielenie odpowiedzialności między poszczególne warstwy aplikacji.
Kontrolery są odpowiedzialne za obsługę żądań użytkownika, komunikację z AgroControlApi oraz przekazywanie otrzymanych danych do odpowiednich widoków. Dane są przesyłane i odbierane w formacie JSON, co zapewnia lekkość komunikacji i ułatwia ewentualną integrację z innymi technologiami w przyszłości.
Warstwa widoku została zrealizowana przy użyciu Razor Views, które łączą kod C# z elementami HTML, umożliwiając dynamiczne generowanie treści i wygodne wyświetlanie danych pochodzących z API.
Dzięki temu interfejs użytkownika jest elastyczny, a jego aktualizacja nie wymaga ingerencji w warstwę logiki biznesowej.
Komunikacja między aplikacją MVC a Web API jest zabezpieczona poprzez autoryzację tokenową.
Podczas logowania użytkownik przesyła dane uwierzytelniające do API, które w odpowiedzi generuje token dostępu. Token ten jest następnie dołączany do nagłówków kolejnych żądań HTTP, co umożliwia API weryfikację uprawnień użytkownika i chroni dane przed nieautoryzowanym dostępem.

**Technologie i rozwiązania**

W projekcie zastosowano szereg technologii i narzędzi wspierających rozwój, czytelność kodu i bezpieczeństwo aplikacji.
Do walidacji danych użyto biblioteki FluentValidation, która pozwala w prosty i przejrzysty sposób definiować reguły walidacji dla modeli, zapewniając poprawność danych wprowadzanych przez użytkownika.
Cała komunikacja z serwerem odbywa się za pośrednictwem protokołu HTTP, a dane wymieniane są w formacie JSON.
Widoki oparte na technologii Razor pozwalają na łączenie logiki prezentacji z HTML w sposób czytelny i efektywny. Dzięki temu dane pobrane z API mogą być w prosty sposób wyświetlane w tabelach, formularzach i listach.
Całość projektu została zaprojektowana tak, aby była łatwa do rozbudowy o nowe funkcjonalności oraz elastyczna w kontekście integracji z dodatkowymi interfejsami użytkownika.
