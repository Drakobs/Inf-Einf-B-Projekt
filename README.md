# Wizard Traveller
**Wizard Traveller** ist ein 2D-Endless-Runner, entwickelt in der Unity Game Engine.  
Das Projekt entstand im Rahmen der Vorlesung **Inf-Einf-B**  im Wintersemester 2025/26 an der Otto-Friedrich-Universität Bamberg.

---

Du spielst einen Zauberer, der auf seinem Besen durch eine dunkle, endlos wirkende Höhle fliegt und unaufhaltsam nach vorne getrieben wird.
Der Rückweg ist versperrt – es gibt nur eine Richtung: hinaus.

Weiche Hindernissen aus und überwinde Abgründe, um möglichst weit aus der Höhle heraus zu gelangen.  
Präzise Steuerung und schnelle Reaktionen sind dabei entscheidend.

---

## Installation

Die fertigen Spielversionen sind über die **GitHub Releases** verfügbar:

- Eine **.apk-Datei** für Android-Geräte  
- Eine **.exe-Datei** für Windows  

Diese können heruntergeladen und entsprechend installiert bzw. ausgeführt werden.

## Projekt in Unity öffnen

Das Projekt wurde mit **Unity Version 6.3000.2f1** erstellt.  
Für das Öffnen des Projekts werden daher folgende Programme benötigt:

- **Unity Hub**
- **Unity 6.3000.2f1**

Nach dem Klonen des Repositories kann das Projekt im **Unity Hub** lokalisiert und anschließend geöffnet werden.

## Hintergrund und Inspiration

**Wizard Traveller** orientiert sich am Spiel [Jetpack Joyride](https://en.wikipedia.org/wiki/Jetpack_Joyride). 

Zu Beginn des Projekts wurde auch überlegt, ein anderes 2D-Genre umzusetzen, z. B. einen Bullet-Hell-Shooter. 
Aus persönlichem Interesse entschieden wir uns jedoch für einen Endless-Runner.

Der Umfang des Projektes wurde bewusst klein gehalten, um die Umsetzung in der vorgegebenen Zeit zu gewährleisten und bei der Projektmesse ein fertiges Spiel präsentieren zu können.

# Verwendete Assets
Für den Großteil der Spielwelt wurde das Asset‑Pack [**Mossy Cavern**](https://maaot.itch.io/mossy-cavern) von **Maaot** verwendet, das animierte Pflanzen, Tiles für die Bodengestaltung, Dekorationen und den Zauberer enthält.  

Zusätzlich wurden eigene Assets erstellt, darunter der Besen des Magiers, Partikeleffekte und die UI‑Elemente.

# Funktionsweise & Code
## Zauberer und zugehörige Objekte
Der Zauberer ist im Code/Unity aufgeteilt und kein einzelnes Objekt. Er ist aufgeteilt in den Zauberer, seinen Besen und das Partikelsystem. Besen und Zauberer haben jeweils eigene Animatoren, seperate Sprites und Skripte bekommen. Dieses Feature haben wir erst gegen Ende des Projekts hinzugefügt, damit der Player den Besen "fallen lassen" konnte, wenn er stirbt. Im Folgenden werden die Objekte und ihre Funktionen kurz separat beschrieben. 

### Player 
Der Player, also der Zauberer ist der steuerbare Charakter im Spiel. Zunächst ist zu sagen, dass der Player sich im Normalfall nicht von seiner Position auf der x-Achse wegbewegt. Der Fortschritt und die Fortbewegung liegen daher rein bei der Map und den einzelnen Sections (siehe Kapitel zu Map & Sections).

Der Player selbst hat durch Unity vorgefertigte Komponenten. Mehrere Collider, die einen Zusammenstoß mit der Welt erkennen, sowie einen Rigidbody2D, der den Player zu einem physischen Objekt macht, dass der Gravitation ausgesetzt ist (Player fällt automatisch).
Auch der Animator und der zugehörige Controller, die gemeinsam für die Animation zuständig sind, die der Player abspielt, sind Teile des Players. Der Grund für die mehrere Collider ist, dass ein Collider an den Füßen des Zauberers ein Signal an den Animator sendet, wodurch der Zauberer zu laufen beginnt.

Wie bereits im Kapitel zum Build erwähnt, haben wir uns für das neue Inputsystem in Unity entschieden. So können mehrere Inputs die gleiche Action auslösen. Außerdem wird so der Code möglichst wenig repetetiv, da die verschiedenen Inputs nicht separat behandelt werden müssen, sondern nur die "FlyAction". 
Drückt der Spieler des Spiels also die Inputs, die der Action zugewiesen sind (South Button am Controller, Spacebar, Touch auf einem Display), so wird dem Zauberer durch den Rigidbody eine Force auf der y-Achse gegeben. So funktioniert generell das fliegen des Zauberers.

Außerdem ist eine CatchUp Funktion eingebaut, die dafür sorgt, dass der Player nach Zwei Sekunden wieder an seine ursprüngliche Position zurückgleiten kann. Das ist notwendig, wenn der Zauberer gegen ein nicht-tödliches Objekt geflogen ist und so auf der x-Achse nach links oder rechts abgekommen ist. Damit das Aufholen nicht ruckartig stattfindet, nutzt das Skript die Mathf.Lerp funktion um mehrere Punkte zwischen der aktuellen Position und der Startposition des Zauberers zu finden. Da die Funktion allerdings nicht exakt zum Ausgangspunkt führt, wird, wenn der Abstand zum Ursprung klein genug ist, die letzte Anpassung ruckartig ausgeführt. Sie ist allerdings für den Spieler nicht zu erkennen. 

### Broom
Zu Beginn des Projekts war der Besen noch Teil des Sprites des Zauberers. Er war also nicht separat und hatte keine Hitbox. Um jedoch zu ermöglichen, dass der Player den Besen fallen lässt, sobald er stirbt und so eine sinnvolle Animation zu erstellen, haben wir am Ende den Besen vom Player getrennt. 
Der Besen hat ebenfalls eine Hitbox (Collider) und einen eigenen Rigidbody2D erhalten, um nach dem Tod mit der Welt kollidieren zu können. Der Besen hat ebenfalls sein eigenes Skript erhalten, in dem er sowohl mit dem Player als auch mit dem Animator kommuniziert. Das Skript kümmert sich allerdings lediglich darum, dass der Besen während des Spiels korrekt animiert ist und solange der Player am Leben ist, keine Hitbox und Kollisionen mit dem Besen möglich sind. 

### Animationen 
Die Animationen sind aufgeteilt in den Zauberer und den Besen. Sie besitzen beide separate Controller und Animationen, die über Variablen und Conditions getriggert werden. Die größte Herausforderung war es, dass nach der Trennung von Zauberer und Besen die Animationen weiterhin synchron ablaufen und der Besen nicht sichtbar ist, wenn der Zauberer auf der Map läuft. Gelöst wurde dies, indem der Besen einen transparenten Sprite anzeigt, solange der Player Kontakt mit dem Boden hat. 
Damit der Besen nicht weiter "fliegt", wenn der Spieler gestorben ist, zeigt der Animator einen einzelnen Frame der Animation des Besens an, sobald der Player stirbt, wobei an dieser Stelle zusätzlich zu erwähnen ist, dass das Sterben ein Event ist. 

Die Neigung, die der Zauberer und der Besen vollziehen, ist nicht im Animator, sondern im Skript des Players zu finden. Je nachdem ob der Zauberer eine y-force nach oben oder unten hat, rotiert also der gesamte Player. So wirkt die Steuerung des Zauberers flüssiger und besser.

Zusätzlich ist zu sagen, dass die Animationen der Pflanzen und auch die des Zauberers aus dem verlinkten Assetpack stammen. Den Besen habe ich selbst gezeichnet und auch animiert. Ebenso wie die Animation des Fliegens. Hierfür habe ich einzelne Frames einer Sprung-Animation des Zauberers wiederverwendet und so manipuliert, dass der Zauberer auf dem Besen etwas wackelig fliegen kann. 

### Particle System
Das Partikelsystem ist ein Objekt aus Unity selbst und kann beinahe beliebig durch die Unity-interne UI verändert und angepasst werden. Zunächst waren die Partikel einfache Quadrate, die farblich angepasst wurden. Letztendlich habe ich noch eine Wolke gezeichnet, die man als Sprite einfügen konnte. Im fertigen Partikelsystem wird diese je länger sie "lebt" größer und ändert ihre Farbe. Der Prozess bis zum fertigen System war hier geprägt von trial-and-error bis das Bild stimmig aussah.

## Spielwelt

Die gesamte Spielwelt wird im Projekt als **World** bezeichnet. Sie besteht aus mehreren Layers — zwei Hintergrund‑Layers und einer Gameplay‑Layer —, die zusammen das visuelle und physische Level ergeben.

### Map (`Map`-Skript)
Das `Map`‑Script ist die zentrale Steuerungsebene.
Es ist für folgende Aufgaben verantwortlich:
- Skalierung und Positionierung der Kamera
- Skalierung und Positionierung des Hintergrund-Nebels der Hintergrund-Layers
- Berechnung der aktuellen Bewegungsgeschwindigkeit.
- Speichern der bereits zurückgelegten Distanz (für Highscore und Bewegungsgeschwindigkeit)

### Layers
Die Welt besteht aus mehreren Layers, die übereinander angeordnet sind:
- Jede Layer bewegt sich selbständig und liest die aktuelle Bewegungsgeschwindigkeit vom `Map`-Script.
- Layers bestehen aus Sections, die aneinandergefügt werden, um ein endloses Level zu bilden.
- Jede Layer ist selbst für das Spawnen neuer Sections und das Entfernen alter Sections verantwortlich.
- Gameplay‑Layer verwendet `MovementLayer` für Sections; Hintergrund‑Layers verwenden `EnvironmentParallaxLayer` mit individuellen Geschwindigkeitsfaktoren für einen visuellen [Parallax-Effekt](https://de.wikipedia.org/wiki/Bewegungsparallaxe#Bewegungsparallaxe_in_Videospielen).

### Sections
Sections sind die Bausteine der einzelnen Layers. Sie sind so gestaltet, dass sie nahtlos aneinandergefügt werden können, um ein kontinuierliches, endloses Level zu schaffen.
- Jede Section ist ein Prefab, das in die Liste `sectionPrefabs` einer `MovementLayer` eingefügt wird.
- StartSections sind spezielle Sections, die als Anfangsabschnitt einer Layer dienen und nicht gespawnt werden, wenn die Layer neue Sections hinzufügt.
- Sections enthalten Hindernisse, die der Spieler überwinden muss, sowie Dekorationen
- Bei der Erstellung von Sections kann sich an bereits existenten Sections orientiert werden, um eine konsistente Gestaltung zu gewährleisten.

### Hindernisse
Hindernisse werden in Sections platziert und können in drei Typen vorkommen:

- **Bewegliche Hindernisse**: nutzen das `MovingObstacle`-Skript als Komponente, in dem eine beliebige Bewegung (z. B. vertikal, horizontal, diagonal) und Geschwindigkeit definiert werden kann.
- **Statische Hindernisse**: verwenden einfache Collider (z. B. *BoxCollider2D* oder *PolygonCollider2D*) zur Hindernis‑Platzierung ohne zusätzliche Logik.
- **Tödliche Hindernisse**: wie statische oder bewegliche Hinernisse, aber mit `DeadlyObstacle`‑Skript als Komponente, das den Spieler bei Kollision tötet.

Alle Hindernisstypen können beliebig kombiniert werden und mit Dekorationen ergänzt werden.

Bei der Erstellung von Hindernissen kann sich an bereits existenten Hindernissen orientiert werden, um eine konsistente Gestaltung zu gewährleisten.
Für bewegliche Hidnernisse existiert ein grundlegendes Prefab unter `Assets/Prefabs/World/Obstacles/MovingObstacle.prefab`.

## User Interface (UI)
Das User Interface (UI) basiert auf einer globalen *Canvas*-Komponente, die vom `UIManager`-Skript verwaltet und dynamisch mit *Popups* gefüllt wird.

### Popup-System
Ermöglicht die dynamische Instanzierung von UI-Popups zur Laufzeit.
#### UI-Manager (`UIManager`-Skript)
Zentrale Verwaltung der UI-Popups und des globalen UI-*Canvas*. Instanziiert Popups basierend auf ihrem Typ aus dem *Resources*-Ordner.

#### Popup
Gruppe von zusammengehörigen UI-Elementen (Menüs, HUD), die als Prefab gespeichert ist: 
- Jedes Popup muss eine Skript-Komponente besitzen, die eine Unterklasse des `Popup`-Skript ist, damit es vom *UIManager* verwaltet werden kann.
- Popup-Prefabs müssen nach dem Typen der `Popup`-Unterklasse benannt werden (z.B. *UIStartMenu*)
- Popup-Prefabs müssen in dem, im *UIManager* spezifizierten Pfad im *Resources*-Ordner liegen (`Resources/Popups`), damit sie vom *UIManager* gefunden und instanziert werden können.

## Szenen-Management
Das Szenen-Management basiert auf der Unity-eigenen *SceneManager* API, die es ermöglicht, zwischen verschiedenen Szenen zu wechseln. Es gibt folgende Szenen:
- **Bootstrap**: Initialisiert das Spiel, lädt die *PersistentScene* und *Game*-Szene und zeigt währenddessen das `UILoadingScreen`-Popup.
- **PersistentScene**: Enthält dauerhafte Manager, das *UI-Canvas* und die Kamera. Bleibt persistent über alle Szenenwechsel hinweg geladen.
- **Game**: Enthält *Spielwelt* und *Player* und führt die eigentliche Gameplay-Logik aus. Wird bei Spielstart und -Restart (neu) geladen.


