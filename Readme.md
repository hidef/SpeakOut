
install tools & restore deps

    cd src/Noti
    npm i
    dotnet restore
    cd ../..

run tests

    cd tests/Noti.Tests
    dotnet restore
    dotnet test
    cd ../..

publish

    cd src/Noti
    serverless deploy