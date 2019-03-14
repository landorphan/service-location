@Check-In
Feature: Windows Tokenizer
	In order to develop a reliable Windows Path parser 
	As a member of the Landorphan Team
	I want to to be able to convert incoming paths into a more managable form

Scenario Outline: Windows tokenizer generates the following tokens
	Given I have the following path: <Path>
	 When I tokenize the path with the 'Windows' tokenizer
	 Then token '0' should be: <Token 0>
     And token '1' should be: <Token 1>
     And token '2' should be: <Token 2>
     And token '3' should be: <Token 3>
     And token '4' should be: <Token 4>
Examples:
# NOTE: Due to Gherkin parsing rules, \ needs to be escaped.  In order to avoid that necissity and
# make the following examples easier to read (`) will be used in place of the (\) character
| Path                               | Token 0    | Token 1  | Token 2  | Token 3  | Token 4 |
| (null)                             | (null)     | (null)   | (null)   | (null)   | (null)  |
| (empty)                            | (empty)    | (null)   | (null)   | (null)   | (null)  |
| C:`                                | C:         | (empty)  | (null)   | (null)   | (null)  |
| C:`dir`file.txt                    | C:         | dir      | file.txt | (null)   | (null)  |
| C:`dir`file.txt`                   | C:         | dir      | file.txt | (empty)  | (null)  |
| C:`dir                             | C:         | dir      | (null)   | (null)   | (null)  |
| C:`dir`                            | C:         | dir      | (empty)  | (null)   | (null)  |
| C:`dir``file.txt                   | C:         | dir      | (empty)  | file.txt | (null)  |
| C:.`file.txt                       | C:.        | file.txt | (null)   | (null)   | (null)  |
| C:.`file.txt`                      | C:.        | file.txt | (empty)  | (null)   | (null)  |
| C:file.txt                         | C:file.txt | (null)   | (null)   | (null)   | (null)  |
| C:file.txt`                        | C:file.txt | (empty)  | (null)   | (null)   | (null)  |
| C:dir                              | C:dir      | (null)   | (null)   | (null)   | (null)  |
| C:dir`                             | C:dir      | (empty)  | (null)   | (null)   | (null)  |
| C:dir`file.txt                     | C:dir      | file.txt | (null)   | (null)   | (null)  |
| C:dir`file.txt`                    | C:dir      | file.txt | (empty)  | (null)   | (null)  |
| ``server`share                     | UNC:server | share    | (null)   | (null)   | (null)  |
| ``server`share`                    | UNC:server | share    | (empty)  | (null)   | (null)  |
| ``server`file.txt                  | UNC:server | file.txt | (null)   | (null)   | (null)  |
| ``server`file.txt`                 | UNC:server | file.txt | (empty)  | (null)   | (null)  |
| ``server`share`dir`file.txt        | UNC:server | share    | dir      | file.txt | (null)  |
| ``server`share`dir`file.txt`       | UNC:server | share    | dir      | file.txt | (empty) |
| ``?`C:`dir`file.txt                | C:         | dir      | file.txt | (null)   | (null)  |
| ``?`C:`dir`file.txt`               | C:         | dir      | file.txt | (empty)  | (null)  |
| ``?`UNC`server`share`dir`file.txt  | UNC:server | share    | dir      | file.txt | (null)  |
| ``?`UNC`server`share`dir`file.txt` | UNC:server | share    | dir      | file.txt | (empty) |
| `dir`file.txt`                     | (empty)    | dir      | file.txt | (empty)  | (null)  |
| .                                  | .          | (null)   | (null)   | (null)   | (null)  |
| .`                                 | .          | (empty)  | (null)   | (null)   | (null)  |
| .`file.txt                         | .          | file.txt | (null)   | (null)   | (null)  |
| .`file.txt`                        | .          | file.txt | (empty)  | (null)   | (null)  |
| .`dir                              | .          | dir      | (null)   | (null)   | (null)  |
| .`dir`                             | .          | dir      | (empty)  | (null)   | (null)  |
| .`dir`file.txt                     | .          | dir      | file.txt | (null)   | (null)  |
| .`dir`file.txt`                    | .          | dir      | file.txt | (empty)  | (null)  |
| ..                                 | ..         | (null)   | (null)   | (null)   | (null)  |
| ..`                                | ..         | (empty)  | (null)   | (null)   | (null)  |
| ..`dir`file.txt                    | ..         | dir      | file.txt | (null)   | (null)  |
| ..`dir`file.txt`                   | ..         | dir      | file.txt | (empty)  | (null)  |
# A Byproduct of the parser means the following will be accepted as a legitimate source
| UNC:server                         | UNC:server | (null)   | (null)   | (null)   | (null)  |
| UNC:server`                        | UNC:server | (empty)  | (null)   | (null)   | (null)  |
| UNC:server`share                   | UNC:server | share    | (null)   | (null)   | (null)  |
| UNC:server`share`                  | UNC:server | share    | (empty)  | (null)   | (null)  |
| UNC:server`share`dir               | UNC:server | share    | dir      | (null)   | (null)  |
| UNC:server`share`dir`              | UNC:server | share    | dir      | (empty)  | (null)  |

