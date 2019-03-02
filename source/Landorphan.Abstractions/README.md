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
| Directory.GetCreationTimeUtc	         | UTC variants not implemented*                                         |
| Directory.GetCurrentDirectory	         | IDirectoryUtilities.GetCurrentDirectory                               |
| Directory.GetDirectories		            | IDirectoryReaderUtilities.GetDirectories                              |
| Directory.GetDirectoryRoot		         | IPathUtilities.GetRootPath                                            |
| Directory.GetFiles	                     | IDirectoryReaderUtilities.GetFiles                                    |
| Directory.GetFileSystemEntries		      | IDirectoryReaderUtilities.GetFileSystemEntries                        |
| Directory.GetLastAccessTime		         | IDirectoryUtilities.GetLastAccessTime                                 |
| Directory.GetLastAccessTimeUtc		      | UTC variants not implemented*                                         |
| Directory.GetLastWriteTime			      | IDirectoryUtilities.GetLastWriteTime                                  |
| Directory.GetLastWriteTimeUtc			   | UTC variants not implemented*                                         |
| Directory.GetLogicalDrives			      | IEnvironmentUtilities.GetLogicalDrives                                |
| Directory.GetParent			            | IPathUtilities.GetParentPath                                          |
| Directory.Move			                  | IDirectoryWriterUtilities.Move                                        |
| Directory.SetCreationTime			      | IDirectoryUtilities.SetCreationTime                                   |
| Directory.SetCreationTimeUtc			   | UTC variants not implemented*                                         |
| Directory.SetCurrentDirectory			   | IDirectoryUtilities.SetCurrentDirectory                               |
| Directory.SetLastAccessTime			      | IDirectoryUtilities.SetLastAccessTime                                 |
| Directory.SetLastAccessTimeUtc			   | UTC variants not implemented*                                         |
| Directory.SetLastWriteTime			      | IDirectoryUtilities.SetLastWriteTime                                  |
| Directory.SetLastWriteTimeUtc			   | UTC variants not implemented*                                         |
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
| File.GetCreationTimeUtc			         | UTC variants not implemented*                                         |
| File.GetLastAccessTime			         | IFileUtilities.GetLastAccessTime                                      |
| File.GetLastAccessTimeUtc			      | UTC variants not implemented*                                         |
| File.GetLastWriteTime			            | IFileUtilities.GetLastWriteTime                                       |
| File.GetLastWriteTimeUtc			         | UTC variants not implemented*                                         |
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
| File.SetCreationTime			            | IFileUtilities.SetCreationTime                                        |
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


* TODO: consider collapsing into one wrapper each:
   * IDirectoryReaderUtilities.EnumerateDirectories           AND   IDirectoryReaderUtilities.GetDirectories
   * IDirectoryReaderUtilities.EnumerateFiles                 AND   IDirectoryReaderUtilities.GetFiles
   * IDirectoryReaderUtilities.EnumerateFileSystemEntries     AND   IDirectoryReaderUtilities.GetFileSystemEntries

