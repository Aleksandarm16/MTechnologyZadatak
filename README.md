# ZadatakApi

This application simulates finding coresponding contacts for given user

With 2 tabels User and Contacts. Contacts table has foreig key for a given User.

If the User has a Contact and that contact nummber is found in User table under a diferent user it indicates for that user that it can send messages to the given contact.

Initialy mock data is seeded to the databese via JSON file.

User Api:

1. CreateUser - Creates a new User

2. DeleteUser - Deletes selected User

3. EditUser - Modify existing User

4. GetAll - Gets all Users in the User table

5. GetAvailableContacts - Returns list of Users for the selected User who can he send messages to

6. SendMessage - Simulates sending message from one user to another if they are both registered

Contact Api:

1. CreateContact - Creates a new Contacts for a User

2. DeleteContact - Deletes existing Contact for a User

3. EditContact - Modify exisitng Contact

4. GetAll - Gets all Contacts from Contact table
