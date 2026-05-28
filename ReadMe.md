# Mtf.Music

A lightweight C# library for representing musical notes, melodies, and timing based on physical sound properties (frequency, wavelength, and musical theory abstractions).

The library focuses on **data-driven musical representation**, not UI or audio playback.

---

## Core Concept

Instead of treating notes as symbolic values only, this library models them using:

- Fundamental frequency (A4 = 440 Hz default)
- Semitone deviation
- Derived frequency using equal temperament tuning
- Physical wavelength based on speed of sound

---

## Features

### 🎵 Musical representation
- Strongly typed notes (`C4`, `D#5`, etc.)
- Support for natural and sharp/flat notes
- Enum-based note duration (`NoteType`)

### 🎼 Melody construction
- Melodies are defined as ordered sequences of notes
- Built-in predefined melodies

### ⏱ Timing system
- Time signature support
- BPM-based timing calculation
- Accurate note duration computation in milliseconds

### 🔊 Physical audio model
- Frequency calculation using:
  - `f = f0 * 2^(n/12)`
- Wavelength calculation:
  - `λ = speedOfSound / frequency`

---

## Project Structure

```

Mtf.Music
│
├── Note                  # Base musical note model
├── Melody                # Sequence of notes with timing
├── TimeSignature        # Beat/bar structure
├── NoteType             # Duration (whole, half, quarter, etc.)
├── MusicalScale         # Note name mapping
│
├── Notes/               # Generated strongly typed notes
│   ├── C4, D4, E4...
│   ├── Cs4_Db4 (sharp/flat aliases)
│
├── Melodies/            # Predefined songs
│   ├── BociBoci
│   ├── JingleBells
│   ├── ImperialMarch
│   ├── NeverGonnaGiveYouUp

````

---

## Example Usage

### Creating a melody

```csharp
using Mtf.Music;
using Mtf.Music.Notes;
using Mtf.Music.Melodies;

var melody = new BociBoci();

Console.WriteLine(melody);
````

---

### Working with notes

```csharp
var note = new C4(NoteType.Eighth);

Console.WriteLine(note.Name);
Console.WriteLine(note.Frequency);
Console.WriteLine((ushort)note); // rounded frequency
```

---

### Timing calculation

```csharp
var melody = new BociBoci();

ushort ms = melody.GetNoteLength(NoteType.Quarter);
```

---

## Design Notes

* `Melody` inherits from `List<Note>` for direct sequence manipulation
* Notes are immutable in identity but configurable via `FundamentalFrequency`
* Default tuning is A4 = 440 Hz, adjustable globally per melody
* Sharp/flat notes are represented as alias classes (e.g. `Cs4_Db4`)

---

## Limitations

* No built-in audio rendering
* No MIDI export
* No real-time synthesis
* No dynamic rhythm quantization

This is a **representation layer only**, intended for further processing or audio backend integration.
