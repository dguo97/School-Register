# School Register

A Windows desktop program for keeping a school's records in one place: pupils, teachers, subjects, clubs, the weekly timetable, uniform needs and the school bus list. It is written in VB.NET (Windows Forms) and stores everything in a single Microsoft Access file.

It was originally built for an international school in Nigeria (creche, nursery and primary classes) and works for other schools with small changes (see [Customising](#customising)).

## Features

- Register, search, edit and delete **pupils** and **teachers** (IDs are shown as `S23` for pupils and `T7` for teachers)
- Pupil details: name, gender, year, date of birth, address details, two phone numbers, email, school bus
- **Subjects**, **clubs** and a Monday to Friday **timetable** (6 periods a day) that warns about room or teacher clashes
- **Assign pupils** to clubs and subjects (a pupil can be in many)
- **Uniform** list: how many of each of 13 items every pupil needs, with totals for the pupils shown
- **School bus list**: pick-up address and distance for each pupil who uses the bus
- Filters by Year, Gender and School bus
- Two kinds of user: **admin** (everything) and **teacher** (day-to-day tasks, own profile, own password)
- Passwords are hashed, and accounts lock for 5 minutes after 5 wrong passwords

## Requirements

- Windows 10 or 11 (.NET Framework 4.8 is included)
- Visual Studio (the free Community edition is fine) with the **.NET desktop development** workload
- The **Microsoft Access Database Engine** (ACE OLEDB). It is already installed if you have Microsoft Office with Access. Otherwise install the 2016 redistributable, in the 32-bit or 64-bit version that matches your program.

## Getting started

1. Download this repository, or clone it with `git clone https://github.com/dguo97/School-Register.git`.
2. Open `SchoolRegister.slnx` (or `SchoolRegister.vbproj`) in Visual Studio.
3. Build and run.
4. On first run, if no `Database.accdb` is found, choose **Yes** to create a new empty one. The program creates all the tables it needs.
5. Open the **Admin** tab and choose an admin password (at least 6 characters).
6. Add subjects and clubs, then teachers, then pupils.

If you see an error mentioning the "provider", set **Project > Properties > Compile > Platform target** to **x86** (32-bit engine) or **x64** (64-bit engine).

## Database

One Access file, `Database.accdb`, kept next to the program (the program also looks up to six folders above it). Tables:

| Table | Holds |
|---|---|
| `studentsdetail` | pupils |
| `teachersdetail` | teachers and their hashed passwords (column `PasswordHash`) |
| `adminpassword` | the hashed admin password (column `PasswordHash`) and its lock-out state |
| `Subjects`, `Clubs` | subjects and clubs |
| `StudentSubjects`, `StudentClubs` | which pupils take which subject or club |
| `Timetable` | which subject is in which day and period |
| `BusList` | bus pick-up address and distance |
| `Uniform` | uniform counts per pupil |

Databases made by an earlier version are upgraded automatically on first run: password hashes are copied into the new `PasswordHash` columns and the old `Runnerpassword` and `ID` columns are emptied (you can delete them in Access). Leftovers from earlier versions are tidied too: the unused teacher `Weight` column is removed, the oddly named `HouseNumber?` and `Subeject` columns are renamed to `HouseNumber` and `Subject`, and two unused empty tables are dropped. Make a backup first.

## Customising

These settings sit together near the top of `Db.vb`:

- `YearLevels`: the choices in the Year drop-down
- `UniformColumns` / `UniformLabels`: the uniform items (changing them also needs matching columns in the `Uniform` table)
- `IsValidPhone`: the phone rule (now 11 digits starting with 0)
- `IsValidPostcode`: the post code rule (now optional, 3 to 10 letters or digits)
- `DistanceUnit`: label for the bus distance (now km)

## Security and privacy

- **Never commit a real database.** It contains pupils' names, phone numbers and addresses. `.gitignore` already excludes `*.accdb`.
- Passwords are stored as salted PBKDF2 hashes, never as plain text.
- This is a small single-user program. It has no audit log and has not been security tested. Keep the database file in a folder only authorised staff can open, and back it up regularly.
- Pupil records are personal data. Follow your school's and country's data protection rules before using this with real information.

## Limits

- One person at a time should use one database file.
- The timetable has 5 days and 6 periods.
- Names can be up to 30 characters.

## Project layout

| File | Purpose |
|---|---|
| `Signin`, `Adminhome`, `TeacherHomepage` | sign-in and the two home pages |
| `RegisterForm` | add or edit a pupil or teacher |
| `ManageForm` | list, search, filter, add, edit and delete screen |
| `EnrolForm`, `TimetableForm`, `UniformForm`, `RecordDialog`, `PasswordPrompt` | the other screens |
| `Db.vb`, `DbSchema.vb` | database access, settings, password hashing, table creation |
| `docs/USER_MANUAL.md` | the user manual |

## Documentation

The full user manual (setup, admin guide, teacher guide, troubleshooting) is in [`docs/USER_MANUAL.md`](docs/USER_MANUAL.md).

## Licence

MIT. See `LICENSE`.
