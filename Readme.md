
## Getting Involved

### install tools & restore deps

    cd src/SpeakOut
    npm i
    dotnet restore
    cd ../..

### run tests

    cd tests/SpeakOut.Tests
    dotnet restore
    dotnet test
    cd ../..

### publish

    cd src/SpeakOut
    serverless deploy

## Friend Linking

Generate Code
A > Give me a friend code!
* Generate Code: 12345=>`{A's UserId}`
A < 12345

Friend adds you with code
B > Remember my friend `{Friend Nick}` with Code `{Code}`
* Get `{Friend's UserId}` from `{Code}`
* Add `{{Friend Nick}={Friend's UserId}}` to `{My UserId}`'s friends
* Add `{{My Nick}={My UserId}}` to `{Friend's UserId}`'s friends
B < I have remembered `{Friend Nick}`


B > Tell `{Friend Nick}` I like cats
* Get `{{Friend Nick}'s UserId}`
* Append Message to `{{Friend Nick}'s UserId}`'s Queue
B < OK

A > Delete my queue
* Delete `{My UserId}`'s Queue

Data:
Name <=> UserId
UserId => UserId[] (friend list)
Code => UserId
UserId => Message[] QUEUE

## pseudo code

Get Code

    Code => User ID

    string code = getCodeFor(myUserId);

Befriend `{name}` with code `{code}`

    dedupe name in my address book
    get UserId From Code

    Add Name=UserId to My friends list
    Add MyName=MyUserId to UserIds friends list (how do dedup?)


    Dictionary<string, string> addressBook = getAddressBook(myUserId);
    if ( addressBook.ContainsKey(name) ) {
        return "You already have a friend called {name}, try a different name so I can tell them apart";
    }

    addressBook[name] = getUserIdFromCode(code);

    return $"I now know {name}"; 


forget `{name}`

    Dictionary<string, string> addressBook = getAddressBook(myUserId);
    if ( addressBook.ContainsKey(name) ) {
        addressBook.Remove(name);
        saveAddressBook(myUserId, addressBook);
        return "Done";
    }

    return $"I don't know anyone called {name}";

tell `{name}` `{message}`

    var friendId = getAddressList(myUserId)[name];
    saveMessage(friendId, message);