# jsMate

#### Overview
jsMate is a web-based chess application built initially over a [time-boxed weekend](https://github.com/ballance/jsmate/graphs/punch-card).

![jsMate screenshot](https://github.com/ballance/jsmate/blob/master/project_resources/jsmate-screenshot_v_0_5.png "jsMate screenshot")


#### Planning / Status
[Trello Board](https://trello.com/b/rEdr94uM/jsmate-kanban-board)

#### Dependencies
| Dependency         | Installation |
| ------------------ | ------------ |
| Node.js            | [Installer](https://nodejs.org/en/download/) |
| connect            | `npm install connect` |
| serve-static       | `npm install serve-static` |
| cors               | `npm install cors` |
| Nancy              | `Install-Package Nancy` |
| Nancy.Hosting.Self | `Install-Package Nancy.Hosting.Self ` |
| LiteDB             | `Install-Package LiteDB`  |


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

##### Helpful tools
 - [LiteDB Shell](https://github.com/mbdavid/LiteDB/releases)
