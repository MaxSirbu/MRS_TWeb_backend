# Integrare Timer Backend - Frontend

## Descriere
Backend-ul suportă acum sesiuni de timer care pot fi salvate și stocate în baza de date. Această documentație explică cum să se integreze funcționalitatea timerului din frontend cu API-ul nou.

## Endpoints disponibili

### 1. Crearea unei sesiuni de timer
**POST** `/api/timer`

**Request Body:**
```json
{
  "durationSeconds": 300,
  "exerciseName": "Push-ups",
  "notes": "Added strength training"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "userId": 1,
  "durationSeconds": 300,
  "startedAt": "2026-05-15T10:30:00Z",
  "completedAt": null,
  "status": "Running",
  "exerciseName": "Push-ups",
  "notes": "Added strength training",
  "createdAt": "2026-05-15T10:30:00Z"
}
```

---

### 2. Obținerea unei sesiuni specific
**GET** `/api/timer/{id}`

**Response (200 OK):**
```json
{
  "id": 1,
  "userId": 1,
  "durationSeconds": 300,
  "startedAt": "2026-05-15T10:30:00Z",
  "completedAt": "2026-05-15T10:35:00Z",
  "status": "Completed",
  "exerciseName": "Push-ups",
  "notes": "Added strength training",
  "createdAt": "2026-05-15T10:30:00Z"
}
```

---

### 3. Listarea tuturor sesiunilor
**GET** `/api/timer?limit=10&offset=0`

**Query Parameters:**
- `limit` (optional): Numărul de rezultate per pagină
- `offset` (optional): Skip offset pentru paginare

**Response (200 OK):**
```json
{
  "total": 15,
  "data": [
    {
      "id": 1,
      "userId": 1,
      "durationSeconds": 300,
      "startedAt": "2026-05-15T10:30:00Z",
      "completedAt": "2026-05-15T10:35:00Z",
      "status": "Completed",
      "exerciseName": "Push-ups",
      "notes": null,
      "createdAt": "2026-05-15T10:30:00Z"
    }
  ]
}
```

---

### 4. Actualizare sesiune (marcare complet, pauză, adăugare note)
**PUT** `/api/timer/{id}`

**Request Body:**
```json
{
  "status": "Completed",
  "notes": "Finalizat cu succes"
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "userId": 1,
  "durationSeconds": 300,
  "startedAt": "2026-05-15T10:30:00Z",
  "completedAt": "2026-05-15T10:35:00Z",
  "status": "Completed",
  "exerciseName": "Push-ups",
  "notes": "Finalizat cu succes",
  "createdAt": "2026-05-15T10:30:00Z"
}
```

**Status values:** `Running`, `Paused`, `Completed`, `Abandoned`

---

### 5. Obținere statistici
**GET** `/api/timer/statistics/overview`

**Response (200 OK):**
```json
{
  "totalSessions": 15,
  "totalSeconds": 4500,
  "completedSessions": 14,
  "averageSessionSeconds": 300,
  "mostUsedExercise": "Push-ups"
}
```

---

### 6. Ștergere sesiune
**DELETE** `/api/timer/{id}`

**Response (204 No Content)**

---

## Integrare Frontend

### Exemplu: Salvare timer completat
```typescript
// După ce timer-ul se termină în frontend
async function saveTimerSession(duration: number, exerciseName?: string) {
  const response = await fetch('/api/timer', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      durationSeconds: duration,
      exerciseName,
      notes: null
    })
  });
  
  if (response.ok) {
    const session = await response.json();
    console.log('Session saved:', session);
  }
}
```

### Exemplu: Obținere istoricul
```typescript
async function getTimerHistory() {
  const response = await fetch('/api/timer?limit=20');
  const data = await response.json();
  console.log('Timer history:', data.data);
}
```

### Exemplu: Obținere statistici
```typescript
async function getStats() {
  const response = await fetch('/api/timer/statistics/overview');
  const stats = await response.json();
  console.log('Total time:', stats.totalSeconds, 'seconds');
}
```

---

## Status enum

- **Running**: Timer-ul este în curs de rulare
- **Paused**: Timer-ul a fost pus pe pauză
- **Completed**: Timer-ul a fost completat complet
- **Abandoned**: Utilizatorul a abandonat sesiunea

---

## Note pentru producție

1. **Autentificare**: În prezent, `userId` este hardcodat (1). În producție, trebuie extras din contextul de autentificare.

2. **Persistență BD**: Controller-ul folosește în prezent storage în memorie (`List<TimerSession>`). Trebuie integrat cu Entity Framework Core și `DbContext` pentru persistență în baza de date.

3. **Validare**: Adăugați validare suplimentară pentru `ExerciseName` și `Notes`.

4. **Rate Limiting**: Implementați rate limiting pentru a preveni abuz de API.

---

## Pornire Backend cu Timer Support

```powershell
cd scripts
.\Run-Api.ps1
```

API va fi disponibil la: `http://localhost:5000` și `https://localhost:5001`

Swagger documentation: `http://localhost:5000/swagger`
