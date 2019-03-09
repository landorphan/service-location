## Mapping
| .Net Standard 2.0                       | Landorphan.Abstractions                                               |
| :-------------------------------------- | :-------------------------------------------------------------------- |
| ---	                                    | IDirectoryWriterUtilities.Copy                                        |
| Directory.CreateDirectory	            | IDirectoryUtilities.CreateDirectory                                   |
| Directory.Delete	                     | IDirectoryUtilities.DeleteEmpty                                       |
| Directory.Delete	                     | IDirectoryUtilities.DeleteRecursively                                 |
| Directory.EnumerateDirectories          | IDirectoryReaderUtilities.EnumerateDirectories                        |
| Directory.EnumerateFiles	               | IDirectoryReaderUtilities.EnumerateFiles                              |
| Directory.EnumerateFileSystemEntries	   | IDirectoryReaderUtilities.EnumerateFileSystemEntries                  |
| Directory.Exists	                     | IDirectoryUtilities.DirectoryExists                                   |
| Directory.GetCreationTime	            | IDirectoryUtilities.GetCreationTime                                   |
| Directory.GetCreationTimeUtc	         | <b>(UTC variants not implemented*)</b>                                         |
| Directory.GetCurrentDirectory	         | IDirectoryUtilities.GetCurrentDirectory                               |
| Directory.GetDirectories		            | IDirectoryReaderUtilities.GetDirectories                              |
| Directory.GetDirectoryRoot		         | IPathUtilities.GetRootPath                                            |
| Directory.GetFiles	                     | IDirectoryReaderUtilities.GetFiles                                    |
| Directory.GetFileSystemEntries		      | IDirectoryReaderUtilities.GetFileSystemEntries                        |
| Directory.GetLastAccessTime		         | IDirectoryUtilities.GetLastAccessTime                                 |
| Directory.GetLastAccessTimeUtc		      | <b>(UTC variants not implemented*)</b>                                         |
| Directory.GetLastWriteTime			      | IDirectoryUtilities.GetLastWriteTime                                  |
| Directory.GetLastWriteTimeUtc			   | <b>(UTC variants not implemented*)</b>                                         |
| Directory.GetLogicalDrives			      | IEnvironmentUtilities.GetLogicalDrives                                |
| Directory.GetParent			            | IPathUtilities.GetParentPath                                          |
| Directory.Move			                  | IDirectoryWriterUtilities.Move                                        |
| Directory.SetCreationTime			      | <b>(REMOVED FROM INTERFACE**)</b> IDirectoryUtilities.SetCreationTime                                   |
| Directory.SetCreationTimeUtc			   | <b>(UTC variants not implemented*)</b>                                         |
| Directory.SetCurrentDirectory			   | IDirectoryUtilities.SetCurrentDirectory                               |
| Directory.SetLastAccessTime			      | IDirectoryUtilities.SetLastAccessTime                                 |
| Directory.SetLastAccessTimeUtc			   | <b>(UTC variants not implemented*)</b>                                         |
| Directory.SetLastWriteTime			      | IDirectoryUtilities.SetLastWriteTime                                  |
| Directory.SetLastWriteTimeUtc			   | <b>(UTC variants not implemented*)</b>                                         |
| File.AppendAllLines			            | IFileWriterUtilities.AppendAllLines                                   |
| File.AppendText			                  | IFileWriterUtilities.AppendAllText                                    |
| File.Copy			                        | IFileWriterUtilities.CopyNoOverwrite                                  |
| File.Copy			                        | IFileWriterUtilities.CopyWithOverwrite                                |
| File.Create			                     | IFileUtilities.CreateFile                                             |
| Path.GetTempFileName			            | IFileUtilities.CreateTemporaryFile                                    |
| File.CreateText			                  | IFileUtilities.CreateText                                             |
| File.Decrypt			                     | NTFS Only - not implemented*                                          |
| File.Delete			                     | IFileUtilities.DeleteFile                                             |
| File.Encrypt			                     | NTFS Only - not implemented*                                          |
| File.Exists			                     | IFileUtilities.FileExists                                             |
| File.GetAttributes			               | IFileUtilities.GetAttributes                                          |
| File.GetCreationTime			            | IFileUtilities.GetCreationTime                                        |
| File.GetCreationTimeUtc			         | <b>(UTC variants not implemented*)</b>                                         |
| File.GetLastAccessTime			         | IFileUtilities.GetLastAccessTime                                      |
| File.GetLastAccessTimeUtc			      | <b>(UTC variants not implemented*)</b>                                         |
| File.GetLastWriteTime			            | IFileUtilities.GetLastWriteTime                                       |
| File.GetLastWriteTimeUtc			         | <b>(UTC variants not implemented*)</b>                                |
| File.Move			                        | IFileWriterUtilities.Move                                             |
| File.Open			                        | IFileReaderUtilities.Open                                             |
| File.OpenRead			                  | IFileReaderUtilities.OpenRead                                         |
| File.OpenText			                  | IFileReaderUtilities.OpenText                                         |
| File.OpenWrite		             	      | IFileWriterUtilities.OpenWrite                                        |
| File.ReadAllBytes	             		   | IFileReaderUtilities.ReadAllBytes                                     |
| File.ReadAllLines	             		   | IFileReaderUtilities.ReadAllLines                                     |
| File.ReadAllText	             		   | IFileReaderUtilities.ReadAllText                                      |
| File.ReadLines		             	      | IFileReaderUtilities.ReadLines                                        |
| File.Replace			                     | IFileWriterUtilities.ReplaceContentsNoBackup                          |
| File.Replace			                     | IFileWriterUtilities.ReplaceContentsNoBackupIgnoringMetadataErrors    |
| File.Replace			                     | IFileWriterUtilities.ReplaceContentsWithBackup                        |
| File.Replace			                     | IFileWriterUtilities.ReplaceContentsWithBackupIgnoringMetadataErrors  |
| File.SetAttributes			               | IFileUtilities.SetAttributes                                          |
| File.SetCreationTime			            | <b>(REMOVED FROM INTERFACE**)</b>IFileUtilities.SetCreationTime       |
| File.SetCreationTimeUtc			         | UTC variants not implemented                                          |
| File.SetLastAccessTime			         | IFileUtilities.SetLastAccessTime                                      |
| File.SetLastAccessTimeUtc			      | UTC variants not implemented                                          |
| File.SetLastWriteTime			            | IFileUtilities.SetLastWriteTime                                       |
| File.SetLastWriteTimeUtc			         | UTC variants not implemented                                          |
| File.WriteAllBytes			               | IFileWriterUtilities.WriteAllBytes                                    |
| File.WriteAllLines			               | IFileWriterUtilities.WriteAllLines                                    |
| File.WriteAllText			               | IFileWriterUtilities.WriteAllText                                     |
| Path.AltDirectorySeparatorChar			   | IPathUtilities.AltDirectorySeparatorCharacter                         |
| Path.ChangeExtension			            | IPathUtilities.ChangeExtension                                        |
| Path.Combine			                     | IPathUtilities.Combine                                                |
| Path.DirectorySeparatorChar			      | IPathUtilities.DirectorySeparatorCharacter                            |
| Path.GetDirectoryName			            | IPathUtilities.GetParentPath                                          |
| Path.GetExtension			               | IPathUtilities.GetExtension                                           |
| Path.GetFileName			               | IPathUtilities.GetFileName                                            |
| Path.GetFileNameWithoutExtension		   | IPathUtilities.GetFileNameWithoutExtension                            |
| Path.GetFullPath			               | IPathUtilities.GetFullPath                                            |
| Path.GetInvalidFileNameChars			   | IPathUtilities.GetInvalidFileNameCharacters                           |
| Path.GetInvalidPathChars			         | IPathUtilities.GetInvalidPathCharacters                               |
| Path.GetPathRoot			               | IPathUtilities.GetRootPath                                            |
| Path.GetRandomFileName			         | IDirectoryUtilities.GetRandomDirectoryName                            |
| Path.GetRandomFileName			         | IFileUtilities.GetRandomFileName                                      |
| Path.GetTempFileName			            | IFileUtilities.CreateTemporaryFile                                    |
| Path.GetTempPath			               | IDirectoryUtilities.GetTemporaryDirectoryPath                         |
| Path.HasExtension			               | IPathUtilities.HasExtension                                           |
| Path.InvalidPathChars		               | IPathUtilities.GetInvalidFileNameCharacters                           |
| Path.IsPathRooted		                  | IPathUtilities.IsPathRelative                                         |
| Path.PathSeparator			               | IPathUtilities.PathSeparatorCharacter                                 |
| Path.VolumeSeparatorChar		            | IPathUtilities.VolumeSeparatorCharacter                               |
| Environment.CommandLine			         | IEnvironmentUtilities.CommandLine                                     |
| Environment.CurrentDirectory			   | IDirectoryUtilities.CurrentDirectory                                  |
| Environment.CurrentManagedThreadId	   | IEnvironmentUtilities.CurrentManagedThreadId                          |
| Environment.Exit			               | IEnvironmentUtilities.Exit                                            |
| Environment.ExitCode			            | IEnvironmentUtilities.ExitCode                                        |
| Environment.ExpandEnvironmentVariables  | IEnvironmentUtilities.ExpandEnvironmentVariables                      |
| Environment.FailFast			            | IEnvironmentUtilities.FailFast                                        |
| Environment.GetCommandLineArgs		      | IEnvironmentUtilities.GetCommandLineArgs                              |
| Environment.GetEnvironmentVariable		| IEnvironmentUtilities.GetEnvironmentVariable                          |
| Environment.GetEnvironmentVariables		| IEnvironmentUtilities.GetEnvironmentVariables                         |
| Environment.GetFolderPath		         | IEnvironmentUtilities.GetSpecialFolderPath                            |
| Environment.GetLogicalDrives			   | IEnvironmentUtilities.GetLogicalDrives                                |
| Environment.HasShutdownStarted		      | IEnvironmentUtilities.HasShutdownStarted                              |
| Environment.Is64BitOperatingSystem		| IEnvironmentUtilities.Is64BitOperatingSystem                          |
| Environment.Is64BitProcess			      | IEnvironmentUtilities.Is64BitProcess                                  |
| Environment.MachineName			         | IEnvironmentUtilities.MachineName                                     |
| Environment.NewLine		               | IEnvironmentUtilities.NewLine                                         |
| Environment.OSVersion			            | IEnvironmentUtilities.OSVersion                                       |
| Environment.ProcessorCount			      | IEnvironmentUtilities.ProcessorCount                                  |
| Environment.SetEnvironmentVariable		| IEnvironmentUtilities.SetEnvironmentVariable                          |
| Environment.StackTrace		            | IEnvironmentUtilities.StackTrace                                      |
| Environment.SystemDirectory			      | IEnvironmentUtilities.SystemDirectory                                 |
| Environment.SystemPageSize			      | IEnvironmentUtilities.SystemPageSizeBytes                             |
| Environment.TickCount			            | IEnvironmentUtilities.TickCount                                       |
| Environment.UserDomainName		         | IEnvironmentUtilities.UserDomainName                                  |
| Environment.UserInteractive			      | IEnvironmentUtilities.UserInteractive                                 |
| Environment.UserName		               | IEnvironmentUtilities.UserName                                        |
| Environment.Version			            | IEnvironmentUtilities.ClrVersion                                      |
| Environment.WorkingSet			         | IEnvironmentUtilities.WorkingSetBytes                                 |

\* UTC Variants not implemented:  All times are converted to UTC by this library.  Unspecified values are consistently 
treated as UTC.


\*\* REMOVED FROM INTERFACE:  Setting creation time proved to be unreliable and untestable, so it has been
removed from the interface.  You are free to call File.SetCreationTime and Directory.SetCreationTime.



* TODO: consider collapsing into one wrapper each:
   * IDirectoryReaderUtilities.EnumerateDirectories           AND   IDirectoryReaderUtilities.GetDirectories
   * IDirectoryReaderUtilities.EnumerateFiles                 AND   IDirectoryReaderUtilities.GetFiles
   * IDirectoryReaderUtilities.EnumerateFileSystemEntries     AND   IDirectoryReaderUtilities.GetFileSystemEntries

