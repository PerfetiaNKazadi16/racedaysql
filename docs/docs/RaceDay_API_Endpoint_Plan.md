 # Ra API Endpoint Plan

Section B – API Endpoint Plan

The RaceDay API is a RESTful API designed to manage road running, walking, and cycling events. The API uses role-based access to provide different functionality for Participants and Organisers.

---

# 1. Authentication API

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |

| POST | `/api/auth/register` | Registers a new user account on the RaceDay system. | None (Public) | `{ email, userName, role, accountType, password }` | 201 Created – User registered successfully |
| POST | `/api/auth/login` | Authenticates a registered user and provides access to the system. | None (Public) | `{ email, password }` | 200 OK – Login successful / 401 Unauthorized – Invalid credentials |

---

# 2. User Profile API – Users

**Database Entity: Users**

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|
| GET | `/api/users/{id}` | Retrieves the profile information of a specific user. | Any (Logged in) | None | 200 OK – User profile / 404 Not Found |
| PUT | `/api/users/{id}` | Updates the profile information of a user. | Any (Logged in) | `{ email, userName, accountType }` | 200 OK – Updated user / 404 Not Found |

**Main Attributes:** `usersID`, `Email`, `userName`, `role`, `AccountType`

---

# 3. Participant API – Participants

**Database Entity: Participant**

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | `/api/participants/{id}` | Retrieves information about a specific participant. | Any (Logged in) | None | 200 OK – Participant details / 404 Not Found |
| PUT | `/api/participants/{id}` | Updates a participant's information. | Participant | `{ participantName }` | 200 OK – Updated participant / 404 Not Found |
| GET | `/api/participants/{id}/entries` | Retrieves all events entered by a participant. | Participant | None | 200 OK – List of entries |
| GET | `/api/participants/{id}/results` | Retrieves the participant's race performance history. | Participant | None | 200 OK – List of results |

**Main Attributes:** `participantID`, `participantName`, `usersID`

---

# 4. Organiser API – Organisers

**Database Entity: Organiser**

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | `/api/organisers/{id}` | Retrieves information about a specific organiser. | Organiser | None | 200 OK – Organiser details / 404 Not Found |
| GET | `/api/organisers/{id}/events` | Retrieves all events managed by an organiser. | Organiser | None | 200 OK – List of events |

**Main Attributes:** `organiserID`, `organiserNumber`, `usersID`

---

# 5. Events API – Events

**Database Entity: Events**

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | `/api/events` | Retrieves all available events. | None (Public) | None | 200 OK – List of events |
| GET | `/api/events/{id}` | Retrieves detailed information about a specific event. | None (Public) | None | 200 OK – Event details / 404 Not Found |
| GET | `/api/events/upcoming` | Retrieves upcoming RaceDay events. | None (Public) | None | 200 OK – List of upcoming events |
| POST | `/api/events` | Creates a new road running, walking, or cycling event. | Organiser | `{ eventName, eventDate, description, location, organiserID }` | 201 Created – Event created |
| PUT | `/api/events/{id}` | Updates an existing event. | Organiser | `{ eventName, eventDate, description, location }` | 200 OK – Event updated / 404 Not Found |
| DELETE | `/api/events/{id}` | Deletes an existing event. | Organiser | None | 204 No Content / 404 Not Found |

**Main Attributes:** `eventID`, `eventName`, `eventDate`, `description`, `location`, `organiserID`

---

# 6. Category API – Categories

**Database Entity: Category**

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | `/api/events/{eventId}/categories` | Retrieves all categories belonging to an event. | None (Public) | None | 200 OK – List of categories |
| GET | `/api/categories/{id}` | Retrieves details of a specific category. | None (Public) | None | 200 OK – Category details / 404 Not Found |
| POST | `/api/events/{eventId}/categories` | Creates a new category for an event. | Organiser | `{ categoryName }` | 201 Created – Category created |
| PUT | `/api/categories/{id}` | Updates an existing event category. | Organiser | `{ categoryName }` | 200 OK – Category updated |
| DELETE | `/api/categories/{id}` | Removes a category from an event. | Organiser | None | 204 No Content / 404 Not Found |

**Main Attributes:** `categoryID`, `categoryName`, `eventID`

---

# 7. Route API – Routes

**Database Entity: Route**

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | `/api/events/{eventId}/routes` | Retrieves all routes associated with an event. | None (Public) | None | 200 OK – List of routes |
| GET | `/api/routes/{id}` | Retrieves information about a specific race route. | None (Public) | None | 200 OK – Route details / 404 Not Found |
| POST | `/api/events/{eventId}/routes` | Creates a route for a specific event. | Organiser | `{ routeName, distance }` | 201 Created – Route created |
| PUT | `/api/routes/{id}` | Updates an existing race route. | Organiser | `{ routeName, distance }` | 200 OK – Route updated |
| DELETE | `/api/routes/{id}` | Deletes a race route. | Organiser | None | 204 No Content / 404 Not Found |

**Main Attributes:** `RouteID`, `EventID`, `RouteName`, `distance`

---

# 8. Entry API – Event Entries

**Database Entity: Entry**

The Entry entity represents the relationship between Participants and Events. It records which participant has entered which event and the entry price.

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | `/api/entries` | Registers a participant for an event. | Participant | `{ participantID, eventID, price }` | 201 Created – Entry created / 409 Conflict – Participant already entered |
| GET | `/api/entries/{id}` | Retrieves details of a specific event entry. | Participant / Organiser | None | 200 OK – Entry details / 404 Not Found |
| GET | `/api/participants/{participantId}/entries` | Retrieves all event entries belonging to a participant. | Participant | None | 200 OK – List of entries |
| GET | `/api/events/{eventId}/entries` | Retrieves all participants entered in an event. | Organiser | None | 200 OK – List of entries |
| DELETE | `/api/entries/{id}` | Cancels a participant's event entry. | Participant | None | 204 No Content / 404 Not Found |

**Main Attributes:** `EntryID`, `ParticipantID`, `EventID`, `PRICE`

**Important Database Rule:** A participant cannot enter the same event more than once because `eventID` and `ParticipantID` form a unique combination.

---

# 9. Result API – Race Results

**Database Entity: Result**

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | `/api/events/{eventId}/results` | Retrieves the race results for a specific event. | None (Public) | None | 200 OK – List of results |
| GET | `/api/results/{id}` | Retrieves a specific participant's race result. | None (Public) | None | 200 OK – Result details / 404 Not Found |
| POST | `/api/results` | Records a participant's result for an event. | Organiser | `{ participantID, eventID, finishingTime, position }` | 201 Created – Result recorded |
| PUT | `/api/results/{id}` | Updates a participant's race result. | Organiser | `{ finishingTime, position }` | 200 OK – Result updated / 404 Not Found |
| DELETE | `/api/results/{id}` | Removes a race result. | Organiser | None | 204 No Content / 404 Not Found |
| GET | `/api/participants/{participantId}/results` | Retrieves the performance history of a participant. | Participant | None | 200 OK – List of results |

**Main Attributes:** `resultID`, `participantID`, `eventID`, `finishingTime`, `position`

---

# 10. API Entity Summary

| Database Entity | API Responsibility |
|---|---|
| Users | Registration, login and user profiles |
| Organiser | Organiser information and event management |
| Participant | Participant information and personal history |
| Events | Creating, viewing, updating and deleting events |
| Category | Managing race categories |
| Route | Managing race routes and distances |
| Entry | Registering participants for events |
| Result | Recording and viewing race results |

---

# Role Summary

### Participant

Participants can:

- Register and log in
- View their profile
- Browse events
- View event categories and routes
- Enter events
- View their event entries
- View their personal race results and performance history

### Organiser

Organisers can:

- Register and log in
- Manage their profile
- Create and manage events
- Create and manage event categories
- Create and manage race routes
- View event entries
- Record and manage participant results

### Public Users

Unauthenticated users can:

- View upcoming events
- View event details
- View categories
- View routes
- View published race results
- Register for a RaceDay account
- Log in
