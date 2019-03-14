@Check-In
Feature: Windows Preparse
	In order to develop a reliable Windows Path parser 
	As a member of the Landorphan Team
	I want to to be able to convert incoming paths into a more managable form

Scenario Outline: Windows Preparse converter creates managable strings for the tokenizer
	Given I have the following path: <Path>
	 When I preparse the path
	 Then the resulting path should read: <Pre-Parsed>
Examples:
# NOTE: Due to Gherkin parsing rules, \ needs to be escaped.  In order to avoid that necissity and
# make the following examples easier to read (`) will be used in place of the (\) character
| Path                               | Pre-Parsed                     | Notes                                   |
| (null)                             | (null)                         | Null String                             |
| (empty)                            | (empty)                        | Empty String                            |
| C:`                                | C:/                            | Drive Root                              |
| C:`dir`file.txt                    | C:/dir/file.txt                | Drive + Dir + File                      |
| C:`dir`file.txt`                   | C:/dir/file.txt/               | Drive + Dir + File + Empty              |
| C:`dir                             | C:/dir                         | Drive + Dir                             |
| C:`dir`                            | C:/dir/                        | Drive + Dir + Empty                     |
| C:.`file.txt                       | C:./file.txt                   | Drive + File                            |
| C:.`file.txt`                      | C:./file.txt/                  | Drive + File + Empty                    |
| C:file.txt                         | C:file.txt                     | Volume + File                           |
| C:file.txt`                        | C:file.txt/                    | Volume + File + Empty                   |
| C:dir                              | C:dir                          | Volume + Dir                            |
| C:dir`                             | C:dir/                         | Volume + Dir + Empty                    |
| C:dir`file.txt                     | C:dir/file.txt                 | Volume + Dir + File                     |
| C:dir`file.txt`                    | C:dir/file.txt/                | Volume + Dir + File + Empty             |
| ``server`share                     | UNC:server/share               | UNC + Share                             |
| ``server`share`                    | UNC:server/share/              | UNC + Share + Empty                     |
| ``server`file.txt                  | UNC:server/file.txt            | this is illegal (but not the preparser) |
| ``server`file.txt`                 | UNC:server/file.txt/           | again illegal                           |
| ``server`share`dir`file.txt        | UNC:server/share/dir/file.txt  | Server + Share + Dir + File             |
| ``server`share`dir`file.txt`       | UNC:server/share/dir/file.txt/ | Server + Share + Dire + File + Empty    |
| ``?`C:`dir`file.txt                | C:/dir/file.txt                | Drive Root + Dir + File                 |
| ``?`C:`dir`file.txt`               | C:/dir/file.txt/               | Drive Root + Dir + File + Empty         |
| ``?`UNC`server`share`dir`file.txt  | UNC:server/share/dir/file.txt  | LONG: UNC + share + dir + file          |
| ``?`UNC`server`share`dir`file.txt` | UNC:server/share/dir/file.txt/ | LONG: UNC + Share + dir + file + Empty  |
| .                                  | .                              | self relative                           |
| .`                                 | ./                             | self relative + Empty                   |
| .`file.txt                         | ./file.txt                     | self relative + file                    |
| .`file.txt`                        | ./file.txt/                    | self relative + file + Empty            |
| .`dir                              | ./dir                          | self relative + dir                     |
| .`dir`                             | ./dir/                         | self relatlve + dir + Empty             |
| .`dir`file.txt                     | ./dir/file.txt                 | self reletive + dir + file              |
| .`dir`file.txt`                    | ./dir/file.txt/                | self relatvie + dir + file + Empty      |
| ..                                 | ..                             | parent                                  |
| ..`                                | ../                            | parent + empty                          |
| ..`dir`file.txt                    | ../dir/file.txt                | parent + reletive + dir + file          |
| ..`dir`file.txt`                   | ../dir/file.txt/               | parent + relative + dir + file + empty  |
# A Byproduct of the parser means the following will be accepted as a legitimate source
| UNC:server                         | UNC:server                     | server (illegal)                        |
| UNC:server`                        | UNC:server/                    | server + empty (illegal)                |
| UNC:server`share                   | UNC:server/share               | server + share (legal)                  |
| UNC:server`share`                  | UNC:server/share/              | server + share + empty (legal)          |
| UNC:server`share`dir               | UNC:server/share/dir           | server + share + dir (legal)            |
| UNC:server`share`dir`              | UNC:server/share/dir/          | server + share + dir + empty (legal)    |

