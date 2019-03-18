@Check-In
Feature: Windows Path Segments
	In order to develop a reliable Windows Path parser 
	As a member of the Landorphan Team
	I want to to be able to convert incoming paths into a more managable form

Scenario Outline: Windows Segmenter generates the following segments
	Given I have the following path: <Path>
         # NOTE: the segmentor does not produce a normalized form.
	 When I segment the Windows path
	 Then segment '0' should be: <Segment 0>
     And segment '1' should be: <Segment 1>
     And segment '2' should be: <Segment 2>
     And segment '3' should be: <Segment 3>
     And segment '4' should be: <Segment 4>
Examples:
# NOTE: Due to Gherkin parsing rules, \ needs to be escaped.  In order to avoid that necissity and
# make the following examples easier to read (`) will be used in place of the (\) character
#
# Path Segment Type Shorthand:
# {N} = NullSegment, {E} = EmptySegment, {R} = RootSegment, {D} = DeviceSegment, {/} = VolumelessRootSegment
# {V} = VolumeRelativeSegment, {U} = UncSegment, {G} = Segment, {.} = SelfSegmentk, {..} = ParentSegment
| Path                               | Segment 0   | Segment 1    | Segment 2    | Segment 3    | Segment 4   | Segment 5  |
| (null)                             | {N} (null)  | {N} (null)   | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| (empty)                            | {E} (empty) | {N} (null)   | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| C:`                                | {R} C:      | {E} (empty)  | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| C:`dir`file.txt                    | {R} C:      | {G} dir      | {G} file.txt | {N} (null)   | {N} (null)  | {N} (null) |
| C:`dir`file.txt`                   | {R} C:      | {G} dir      | {G} file.txt | {E} (empty)  | {N} (null)  | {N} (null) |
| C:`dir                             | {R} C:      | {G} dir      | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| C:`dir`                            | {R} C:      | {G} dir      | {E} (empty)  | {N} (null)   | {N} (null)  | {N} (null) |
| C:`dir``file.txt                   | {R} C:      | {G} dir      | {E} (empty)  | {G} file.txt | {N} (null)  | {N} (null) |
| C:.`file.txt                       | {V} C:      | {.} .        | {G} file.txt | {N} (null)   | {N} (null)  | {N} (null) |
| C:.`file.txt`                      | {V} C:      | {.} .        | {G} file.txt | {E} (empty)  | {N} (null)  | {N} (null) |
| C:file.txt                         | {V} C:      | {G} file.txt | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| C:file.txt`                        | {V} C:      | {G} file.txt | {E} (empty)  | {N} (null)   | {N} (null)  | {N} (null) |
| C:dir                              | {V} C:      | {G} dir      | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| C:dir`                             | {V} C:      | {G} dir      | {E} (empty)  | {N} (null)   | {N} (null)  | {N} (null) |
| C:dir`file.txt                     | {V} C:      | {G} dir      | {G} file.txt | {N} (null)   | {N} (null)  | {N} (null) |
| C:dir`file.txt`                    | {V} C:      | {G} dir      | {G} file.txt | {E} (empty)  | {N} (null)  | {N} (null) |
| ``server`share                     | {U} server  | {G} share    | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| ``server`share`                    | {U} server  | {G} share    | {E} (empty)  | {N} (null)   | {N} (null)  | {N} (null) |
| ``server`file.txt                  | {U} server  | {G} file.txt | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| ``server`file.txt`                 | {U} server  | {G} file.txt | {E} (empty)  | {N} (null)   | {N} (null)  | {N} (null) |
| ``server`share`dir`file.txt        | {U} server  | {G} share    | {G} dir      | {G} file.txt | {N} (null)  | {N} (null) |
| ``server`share`dir`file.txt`       | {U} server  | {G} share    | {G} dir      | {G} file.txt | {E} (empty) | {N} (null) |
| ``?`C:`dir`file.txt                | {R} C:      | {G} dir      | {G} file.txt | {N} (null)   | {N} (null)  | {N} (null) |
| ``?`C:`dir`file.txt`               | {R} C:      | {G} dir      | {G} file.txt | {E} (empty)  | {N} (null)  | {N} (null) |
| ``?`UNC`server`share`dir`file.txt  | {U} server  | {G} share    | {G} dir      | {G} file.txt | {N} (null)  | {N} (null) |
| ``?`UNC`server`share`dir`file.txt` | {U} server  | {G} share    | {G} dir      | {G} file.txt | {E} (empty) | {N} (null) |
| `dir`file.txt`                     | {E} (empty) | {/} dir      | {G} file.txt | {E} (empty)  | {N} (null)  | {N} (null) |
| .                                  | {.} .       | {N} (null)   | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| .`                                 | {.} .       | {E} (empty)  | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| .`file.txt                         | {.} .       | {G} file.txt | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| .`file.txt`                        | {.} .       | {G} file.txt | {E} (empty)  | {N} (null)   | {N} (null)  | {N} (null) |
| .`dir                              | {.} .       | {G} dir      | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| .`dir`                             | {.} .       | {G} dir      | {E} (empty)  | {N} (null)   | {N} (null)  | {N} (null) |
| .`dir`file.txt                     | {.} .       | {G} dir      | {G} file.txt | {N} (null)   | {N} (null)  | {N} (null) |
| .`dir`file.txt`                    | {.} .       | {G} dir      | {G} file.txt | {E} (empty)  | {N} (null)  | {N} (null) |
| ..                                 | {..} ..     | {N} (null)   | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| ..`                                | {..} ..     | {E} (empty)  | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| ..`dir`file.txt                    | {..} ..     | {G} dir      | {G} file.txt | {N} (null)   | {N} (null)  | {N} (null) |
| ..`dir`file.txt`                   | {..} ..     | {G} dir      | {G} file.txt | {E} (empty)  | {N} (null)  | {N} (null) |
# Device paths should resolve to a device but the unnormalized segments will still be present
| CON                                | {D} CON     | {N} (null)   | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| C:`CON                             | {R} C:      | {D} CON      | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| ..`CON                             | {..} ..     | {D} CON      | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| `dir`CON                           | {E} (empty) | {/} dir      | {D} CON      | {N} (null)   | {N} (null)  | {N} (null) |
# A Byproduct of the parser means the following will be accepted as a legitimate source                                  
| UNC:server                         | {U} server  | {N} (null)   | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| UNC:server`                        | {U} server  | {E} (empty)  | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| UNC:server`share                   | {U} server  | {G} share    | {N} (null)   | {N} (null)   | {N} (null)  | {N} (null) |
| UNC:server`share`                  | {U} server  | {G} share    | {E} (empty)  | {N} (null)   | {N} (null)  | {N} (null) |
| UNC:server`share`dir               | {U} server  | {G} share    | {G} dir      | {N} (null)   | {N} (null)  | {N} (null) |
| UNC:server`share`dir`              | {U} server  | {G} share    | {G} dir      | {E} (empty)  | {N} (null)  | {N} (null) |
