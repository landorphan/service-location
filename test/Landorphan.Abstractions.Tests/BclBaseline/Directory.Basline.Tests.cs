namespace Landorphan.Abstractions.Tests.BclBaseline
{
    using Landorphan.TestUtilities;

    // ReSharper disable InconsistentNaming

   public class Directory_Baseline_Tests : TestBase
   {
       // MAPPING:
       // Directory:                    IDirectoryUtilities:       IDirectoryReaderUtilities:    IDirectoryWriterUtilities:                 
       // -----------------------------------------------------------------------------------------------------------------
       // --                            --                         --                            Copy
       // CreateDirectory               CreateDirectory            --                            --
       // Delete                        DeleteEmpty                --                            --
       //                               DeleteRecursively          --                            --
       // EnumerateDirectories          --                         EnumerateDirectories          --
       // EnumerateFiles                --                         EnumerateFiles                --
       // EnumerateFileSystemEntries    --                         EnumerateFileSystemEntries    --
       // Exists                        DirectoryExists            --                            --
       // GetCreationTime               GetCreationTime            --                            --
       // GetCreationTimeUtc            (not needed)               --                            --
       // GetCurrentDirectory           GetCurrentDirectory        --                            --
       // GetDirectories                --                         GetDirectories                --
       // GetDirectoryRoot              --                         --                            --    see   IPathUtilities.GetRootPath
       // GetFiles                      --                         GetFiles                      --
       // GetFileSystemEntries          --                         GetFileSystemEntries          --
       // GetLastAccessTime             GetLastAccessTime          --                            --
       // GetLastAccessTimeUtc          (not needed)               --                            --
       // GetLastWriteTime              GetLastWriteTime           --                            --
       // GetLastWriteTimeUtc           (not needed)               --                            --
       // GetLogicalDrives              (see IEnvironmentUtilities.GetLogicalDrives)
       // GetParent                     (see IPathUtilities.GetParentPath)
       // (From Path.GetRandomFileName) GetRandomDirectoryName     --                            --
       // (From Path.GetTempPath)       GetTemporaryDirectoryPath  --                            --
       // Move                          --                         --                            Move
       // SetCreationTime               SetCreationTime            --                            --
       // SetCreationTimeUtc            (not needed)               --                            --
       // SetCurrentDirectory           SetCurrentDirectory        --                            --
       // SetLastAccessTime             SetLastAccessTime          --                            --
       // SetLastAccessTimeUtc          (not needed)               --                            --
       // SetLastWriteTime              SetLastWriteTime           --                            --
       // SetLastWriteTimeUtc           (not needed)               --                            --

       // These tests document what is:  test failures means an implementation detail has changed
       // change the assertion to document "what is"
       // if you believe the behavior to be incorrect, modify the behavior of the abstraction, fix the abstraction tests, and update these documentation tests
       // to show "what is"

       // currently stripped; porting changed too much.
   }
}
