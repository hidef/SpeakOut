
## Getting Involved

### install tools & restore deps

    cd src/Noti
    npm i
    dotnet restore
    cd ../..

### run tests

    cd tests/Noti.Tests
    dotnet restore
    dotnet test
    cd ../..

### publish

    cd src/Noti
    serverless deploy


##

Get Code

    Code => User ID

    string code = getCodeFor(myUserId);

Befriend {name} with code {code}

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


forget {name}

    Dictionary<string, string> addressBook = getAddressBook(myUserId);
    if ( addressBook.ContainsKey(name) ) {
        addressBook.Remove(name);
        saveAddressBook(myUserId, addressBook);
        return "Done";
    }

    return $"I don't know anyone called {name}";

tell {name} {message}

    var friendId = getAddressList(myUserId)[name];
    saveMessage(friendId, message);