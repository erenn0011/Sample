# Parking Garage Attendant Console (Async Edition)

A simple .NET 8 console application built in the same style as the Coffee Shop application.

## Features

- 3 parking levels with configurable capacity (defaults: 20 / 15 / 10)
- Motorcycle: any level
- Car: level 1 or 2
- Van: level 1 only
- Lowest-level, lowest-numbered available spot assignment
- Human-readable tickets such as T-0001
- Entry and exit gate operations run asynchronously
- `CancellationToken` for shutdown and gate cancellation
- `Channel<CompletedStay>` for asynchronous CSV persistence
- CSV file: `CompletedStays.csv`
- Exit using ticket ID or license plate
- Started-15-minute-block fee calculation
- Daily fee cap

## Project structure

```text
ParkingGarageApp
├── Enums
├── Models
├── Repository
├── Services
├── View
└── Program.cs
```

## Run

```bash
dotnet run
```

The application asks for the capacity of each level. Press Enter to use the default.

## Important async concepts used

- `async` / `await`
- `Task`
- `Task.Delay(..., CancellationToken)`
- `Channel<T>`
- `CancellationTokenSource`
- `SemaphoreSlim`
- asynchronous file APIs such as `File.AppendAllTextAsync`
- `Interlocked.Increment`

The parking spot assignment uses a small `lock` only around shared in-memory state. It does not hold the lock during gate delays or file I/O.
