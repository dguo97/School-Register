# School Register User Manual

*School Information System: pupils, teachers, subjects, clubs and timetable*

## 1. What School Register does

School Register is a Windows program for keeping a school's records in one place. With it you can:

- register pupils and teachers, and correct their details later
- keep a list of subjects and clubs
- assign pupils to the clubs and subjects they take
- build and view a weekly timetable (Monday to Friday, 6 periods a day)
- keep each pupil's uniform needs and school bus details

There are two kinds of user. The **administrator** can do everything. A **teacher** can do the day-to-day tasks but cannot delete records or change the school set-up.

All information is stored in one Access file called **Database.accdb**.

### 1.1 Background

School Register was built for an international school in Nigeria that teaches children from the preschool and nursery years through the primary grades.

School Register brings together the records the school handles every day: pupil details, the uniform each pupil needs, who rides the school bus, the clubs and subjects pupils take, and the timetable. It follows the school's own design document, which set out the pupil record, the uniform table, the bus list, the club list and the teacher pages, and it keeps all of them in one Access database.

### 1.2 Using School Register at another school

School Register is not tied to one school. Pupils, teachers, subjects, clubs, the timetable, uniform and bus lists work the same anywhere. A few settings suit the school it was built for and may need a small change elsewhere:

- **Year list.** Creche, Preschool 1 and 2, Nursery 1 and 2, Grade 1 to 6.
- **Telephone numbers.** 11 digits starting with 0, as used in Nigeria and the UK.
- **Post code.** Optional. Any 3 to 10 letters or digits is accepted, so Nigerian, UK and US codes all work.
- **Distance unit.** Kilometres.
- **Uniform items.** The 13 items, and at most 5 of any one item per pupil.

These settings sit together at the top of the program file **Db.vb**, and whoever maintains School Register can change them in a few minutes. Changing the Year list, the telephone rule, the post code rule or the distance unit needs no change to the database. Changing the uniform items also needs matching columns in the Uniform table.

## 2. Installing and first run

### 2.1 What you need

- A Windows computer.
- The **Microsoft Access Database Engine** (ACE OLEDB). It is already there if Microsoft Access or Office is installed. If you see a message mentioning the "provider", install the version that matches your program (32-bit or 64-bit).
- The file **Database.accdb**.

### 2.2 Where the database file goes

Put Database.accdb in the same folder as the program, or in the project folder. School Register looks in the program's folder and up to six folders above it. If it can't find the file, it asks whether to create a new, empty database in the program folder (choose this the first time, for a brand-new school) or to let you choose an existing file. Pick the file once and School Register uses it for that session.

### 2.3 Before the very first run

1. **Make a backup copy of Database.accdb.** The first time School Register starts, it adds a few tables and columns it needs. It only adds things and never changes or deletes existing records. It also makes the first-name and second-name fields longer (30 characters), which does not alter any name already saved. If your database comes from an older version, it also removes an unused weight column and two empty, unused tables, and gives two oddly named columns tidier names.
2. **Close Database.accdb in Microsoft Access.** School Register cannot upgrade the file while it is open elsewhere.

### 2.4 Setting up a new school: recommended order

| Step | Who | What to do |
| --- | --- | --- |
| 1 | Admin | Sign in on the **Admin** tab and choose an admin password (section 3.2) |
| 2 | Admin | Add the **Subjects** (section 5.4) |
| 3 | Admin | Add the **Clubs** (section 5.5) |
| 4 | Admin | Register the **Teachers** and tell each one their Teacher ID and password (section 5.3) |
| 5 | Admin or teacher | Register the **Pupils** (section 5.2) |
| 6 | Admin or teacher | **Assign pupils** to clubs and subjects (section 6) |
| 7 | Admin | Build the **Timetable** (section 7) |

## 3. Signing in

The sign-in screen has two tabs: **Teachers** and **Admin**.

### 3.1 Teacher sign-in

1. Type your **Teacher ID**, for example `T7`. Typing `t7` or just `7` also works.
2. Type your password.
3. Press **Sign in** (or the Enter key).

A pupil ID such as `S7` is not accepted here. If your ID or password is wrong, the same message appears either way, so nothing is given away.

### 3.2 Admin sign-in

1. Click the **Admin** tab.
2. **First time only:** School Register asks you to choose an admin password (at least 6 characters) and to type it again to confirm. If you cancel, you go back to the Teachers tab.
3. Type the admin password and press the admin log-in button.

**Changing the admin password:** on the Admin tab, click the *change password* label. Enter the current password, then the new one twice.

### 3.3 Lock-out

After **5 wrong passwords in a row**, that account is locked for **5 minutes**. This applies to teachers and to the admin. The lock is saved in the database, so closing and reopening the program does not clear it. A correct password resets the count. For a teacher, the administrator can unlock the account straight away by resetting their password (section 5.3).

## 4. Understanding IDs

Every pupil and teacher gets a number automatically when they are saved. On screen it is shown with a letter:

- Teachers: **T** and the number, for example **T7**
- Pupils: **S** and the number, for example **S23**

School Register shows the ID in a message after you save a new person. Teachers need their Teacher ID to sign in, so make a note of it.

## 5. Administrator guide

After admin sign-in you see the **Admin home** page with these buttons: Pupils, Teachers, Subjects, Clubs, Timetable, Assign pupils, Uniform, School bus list and Log out.

### 5.1 The list screen (the same for pupils, teachers, subjects and clubs)

Each list has the same controls:

- **Search box.** Type anything, such as part of a name, a phone number or an ID. The list narrows to rows that contain that text in any column. Clear the box to see everything.
- **Add.** Create a new record.
- **Edit.** Change the selected record. Double-clicking a row does the same.
- **Delete.** Remove the selected record. School Register asks you to confirm, and **this cannot be undone**.
- **Reset password.** Teachers list only (section 5.3).
- **Close.**

The number of matching records is shown above the list.

**Filters.** On the Pupils list, three drop-down boxes next to the search box narrow the list by **Year**, **Gender** and **School bus**. They work together with the search box. The school bus list and the uniform list have a Year filter.

### 5.2 Pupils

Click **Add** (or **Edit**) on the Pupils list. The form asks for:

| Field | Rules |
| --- | --- |
| First name | Required. 2 to 30 characters: letters, spaces, hyphens (-) and apostrophes (') only |
| Second name | Required. Same rules as the first name |
| Gender | Required. Male or Female |
| DOB | Required. Date of birth. For a new pupil, between 0 and 20 years old |
| Year | Required. Chosen from the list: Creche, Preschool 1 and 2, Nursery 1 and 2, Grade 1 to 6 |
| House number | Required. A whole number from 1 to 999 |
| Post code | Optional. 3 to 10 letters or digits |
| Phone number | Required. 11 digits starting with 0 |
| Phone number 2 | Optional. Same rule as the phone number |
| Email | Optional. Must look like name@example.com |
| School bus | Tick the box if the pupil uses the school bus. Two more boxes appear: the pick-up address (required) and the distance to school in km (starts at 2) |

Press **Save**. If something is wrong, the affected boxes turn pink and a message lists everything to correct. If a pupil with the same name and date of birth already exists, School Register asks whether to save anyway.

After saving a new pupil, a message shows their **Student ID** (for example S23).

A new pupil is also given a uniform record with one of every item (section 5.7).

### 5.3 Teachers

The Teachers form is the same as the pupil form, with two differences:

- There is no school bus box. Instead, a new teacher gets a **password** (at least 6 characters, typed twice).
- A new teacher must be at least 18 years old.

After saving, School Register shows the **Teacher ID**. Give it to the teacher together with their password. Teachers can change their own password later (section 8).

**Editing a teacher** changes their details only, not their password.

**Resetting a password.** Select the teacher, press **Reset password**, then type the new password twice. This also unlocks the account if it was locked.

### 5.4 Subjects

On the Subjects list, **Add** or **Edit** shows a small box with three fields: **Subject name**, **Teacher** and **Room**. The first field is required. The teacher is just text, so type the name the way you want it to appear on the timetable.

### 5.5 Clubs

Clubs have four fields: **Club name** (required), **Teacher**, **Location** and **Days**.

### 5.6 What deleting does

- Deleting a **pupil** also removes them from every club and subject, and removes their uniform and school bus records.
- Deleting a **subject** also removes it from the timetable and from pupils' subject lists.
- Deleting a **club** also removes it from pupils' club lists.
- Deleting a **teacher** removes only the teacher's account. Their name in subjects and clubs is plain text and stays as it is.

### 5.7 Uniform

The **Uniform** button shows every pupil with how many of each of 13 uniform items they need: Girls Dress and Top, Boys Trouser and Top, Sky blue top, Dress, Trouser, Skirt, PE Short and Top, PE Short, PE Top, Tie, Badge, Tank top, and Sweater or Jumper.

- Select a pupil and press **Edit uniform** (or double-click the row) to change the numbers, from 0 to 5 for each item.
- **All = 1 (new pupil)** sets every item to 1, and **Clear all** sets every item to 0.
- **Totals (rows shown)** adds up each item for the pupils currently shown. Filter by Year first to get the total for one year, for example when placing a uniform order.
- New pupils start with one of every item. A pupil with no uniform record counts as 0.

### 5.8 School bus list

The **School bus list** shows the pupils who use the school bus, with their name, year, phone number, pick-up address and distance in km. It is read-only, and the Year filter narrows it down.

To change a pupil's bus details, select them and press **Edit bus details**. This opens the pupil's own record. Untick **School bus** to take a pupil off the list.

## 6. Assigning pupils to clubs and subjects

Available to the admin and to teachers through the **Assign pupils** button.

1. Choose **Club** or **Subject**.
2. Pick the club or subject from the drop-down list. Pupils already assigned are ticked automatically.
3. Tick or untick pupils in the list.
4. Press **Save**.

Saving replaces the whole list for that club or subject with whatever is ticked, so untick someone to remove them. A pupil can be in as many clubs and subjects as you like.

If the drop-down list is empty, add subjects or clubs first (sections 5.4 and 5.5).

## 7. The timetable

The timetable is a grid with the days across the top and periods 1 to 6 down the side. Each cell shows the subject, with its teacher and room.

### 7.1 Adding a subject to a slot (admin only)

1. Choose the **subject**, the **day** and the **period** in the bar at the top.
2. Press **Add to timetable**.

School Register warns you if the same **room** or the same **teacher** is already used in that slot, and lets you decide whether to add it anyway. The same subject cannot be added to the same slot twice.

### 7.2 Clearing a slot (admin only)

Click a cell in the grid and press **Clear selected slot**. This removes **everything** in that slot after you confirm.

### 7.3 Viewing (teachers)

Teachers see the same grid without the editing bar.

## 8. Teacher guide

After signing in, the **Teacher home** page shows your name and Teacher ID in its title bar. The buttons are:

- **Students.** View, search and filter pupils by Year, Gender and School bus. You can add new pupils and edit existing ones (section 5.2), but not delete them.
- **Subjects** and **Clubs.** View and search only.
- **Timetable.** View only.
- **Assign pupils.** Same as section 6.
- **Uniform.** View and edit how many of each uniform item a pupil needs (section 5.7).
- **School bus list.** View the pupils who use the bus and edit their bus details (section 5.8).
- **My profile.** View and change your own name, date of birth, house number and phone number. Your password is changed separately.
- **Change my password.** Enter your current password, then your new one (at least 6 characters) twice.
- **Log out.** Returns to the sign-in screen.

## 9. Who can do what

| Task | Teacher | Admin |
| --- | --- | --- |
| View pupils, subjects, clubs and timetable | Yes | Yes |
| Add and edit pupils | Yes | Yes |
| Delete pupils | No | Yes |
| View and edit uniform and school bus details | Yes | Yes |
| Assign pupils to clubs and subjects | Yes | Yes |
| Add, edit and delete subjects and clubs | No | Yes |
| Edit the timetable | No | Yes |
| Add, edit and delete teachers | No | Yes |
| Reset a teacher's password | No | Yes |
| Edit own profile details | Yes | No |
| Change own password | Yes | Yes (from the sign-in screen) |

## 10. Keeping your data safe

- **Back up Database.accdb regularly**, for example by copying it to another folder or drive once a week. Deleted records cannot be recovered without a backup.
- Passwords are never stored as plain text, so nobody, including the administrator, can look one up. They can only be reset.
- Choose passwords that are not easy to guess, and do not share them.
- Pupil records contain personal information. Only give accounts to people who need access, and log out when you leave the computer.

## 11. Troubleshooting

| Problem | What to do |
| --- | --- |
| A message mentions the "provider" or "ACE OLEDB" | Install the Microsoft Access Database Engine that matches the program (32-bit or 64-bit). If you downloaded the x64 zip but have 32-bit Office, use the x86 zip instead, and the other way round. The error message says whether the program is 32-bit or 64-bit |
| School Register asks you to locate Database.accdb | Browse to the file. To avoid this, keep the file next to the program or in the project folder |
| "Database error" when School Register starts | Close Database.accdb if it is open in Access, then start School Register again |
| "Teacher ID or password is incorrect" | Check the ID (for example T7) and the password. The message is the same for both on purpose |
| "Account locked" | Wait for the time shown, or ask the administrator to reset your password |
| Admin login is locked | Wait about 5 minutes |
| A teacher forgot their password | Admin: Teachers list, select the teacher, **Reset password** |
| The admin forgot the admin password | There is no reset in the program. Open Database.accdb in Access, open the table **adminpassword**, delete its single row, and save. The next time you open the Admin tab, School Register asks you to choose a new password. Make a backup first |
| Assign pupils or Timetable lists are empty | Add subjects and clubs first, and pupils before assigning |
| "Please correct the following" when saving | Read the list in the message. The pink boxes show where the problem is |

## 12. Limits to be aware of

- First and second names can be up to 30 characters.
- The teacher shown against a subject or club is typed text, not linked to a teacher account.
- The timetable has 5 days and 6 periods per day.
- The Year list, the uniform items and the telephone, post code and distance rules are set inside the program (section 1.2). Changing them needs a small edit by whoever maintains School Register.
- School Register is designed for one person at a time working on one Database.accdb file. If several people need to use it at once, ask your IT support how to arrange it.

---

*School Register User Manual. Keep this document with your backup of the database.*
