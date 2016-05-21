# jsmate

#### Overview
jsMate is a web-based chess application built initially over a time-boxed weekend.

#### Planning / Status
[Trello Board](https://trello.com/b/rEdr94uM/jsmate-kanban-board)

#### Dependencies (* = coming soon)
 - Node.js
 - Connect
 - Serve-Static
 - Nancy
 - Nancy.Hosting.Self
 - *SQLite

#### Getting started from zero

##### Starting the node Server
~~~~~~~~~~~~~~~~~~~
git clone https://github.com/ballance/jsmate.git
cd jsmate
npm install connect
npm install serve-static
start.bat or start.ps1
~~~~~~~~~~~~~~~~~

##### Set up ACLs for NancyFX
~~~~~~~~~~~~~~~~~
netsh http add urlacl url=http://+:9997/ user=Everyone
~~~~~~~~~~~~~~~~~
source: https://msdn.microsoft.com/en-us/library/ms733768.aspx

##### Run NancyFX for API
~~~~~~~~~~~~~~~~~~
JsMate.Api.exe
~~~~~~~~~~~~~~~~~~
