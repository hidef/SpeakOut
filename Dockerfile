FROM microsoft/dotnet:1.1.1-sdk

ENV NODE_VERSION 7.10.0
RUN apt-get update 

RUN apt-get install xz-utils
RUN curl -SLO "https://nodejs.org/dist/v7.10.0/node-v7.10.0-linux-x64.tar.xz" 
RUN tar -xf "node-v7.10.0-linux-x64.tar.xz" -C /usr/local --strip-components=1 
RUN rm "node-v7.10.0-linux-x64.tar.xz"  
RUN ln -s /usr/local/bin/node /usr/local/bin/nodejs

RUN mkdir /appdir
WORKDIR /appdir
COPY . .
RUN dotnet restore /appdir/src/Noti
RUN dotnet restore /appdir/test/Noti.Tests
WORKDIR /appdir/src/Noti
RUN npm i
RUN dotnet build


