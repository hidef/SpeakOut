
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

## Friend Linking

Generate Code
A > Give me a friend code!
* Generate Code: 12345=>{A's UserId}
A < 12345

Friend adds you with code
B > Remember my friend {Friend Nick} with Code {Code}
* Get {Friend's UserId} from {Code}
* Add {{Friend Nick}={Friend's UserId}} to {My UserId}'s friends
* Add {{My Nick}={My UserId}} to {Friend's UserId}'s friends
B < I have remembered {Friend Nick}


B > Tell {Friend Nick} I like cats
* Get {{Friend Nick}'s UserId}
* Append Message to {{Friend Nick}'s UserId}'s Queue
B < OK

A > Delete my queue
* Delete {My UserId}'s Queue

Data:
Name <=> UserId
UserId => UserId[] (friend list)
Code => UserId
UserId => Message[] QUEUE

