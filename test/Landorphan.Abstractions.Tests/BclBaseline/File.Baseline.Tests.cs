namespace Landorphan.Abstractions.Tests.BclBaseline
{
    using System.IO;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

   public static class File_Baseline_Tests
   {
       // MAPPING:
       // File:                   IFileUtilities:         IFileReaderUtilities: IFileWriterUtilities
       // ------------------------------------------------------------------------------------------
       // AppendAllLines          --                      --                      AppendAllLines
       // AppendText              --                      --                      AppendAllText
       // Copy                    --                      --                      CopyNoOverwrite
       // --                      --                      --                      CopyWithOverwrite
       // Create                  CreateFile              --                      --
       // --                      CreateTemporaryFile     --                      --
       // CreateText              CreateText              --                      --
       // Decrypt                 (NTFS only - not implementing)
       // Delete                  DeleteFile              --                      --
       // Encrypt                 (NTFS only - not implementing)
       // Exists                  FileExists              --                      --
       // GetAttributes           GetAttributes           --                      --
       // GetCreationTime         GetCreationTime         --                      --
       // GetCreationTimeUtc      (not needed)
       // GetLastAccessTime       GetLastAccessTime       --                      --
       // GetLastAccessTimeUtc    (not needed)
       // GetLastWriteTime        GetLastWriteTime        --                      --
       // GetLastWriteTimeUtc     (not needed)
       // --                      GetRandomFileName       --                      --
       // Move                    --                      --                      Move
       // Open                    --                      Open                    --
       // OpenRead                --                      OpenRead                --
       // OpenText                --                      OpenText                --
       // OpenWrite               --                      --                      OpenWrite
       // ReadAllBytes            --                      ReadAllBytes            --
       // ReadAllLines            --                      ReadAllLines            --
       // ReadAllText             --                      ReadAllText             --
       // ReadLines               --                      ReadLines               --
       // Replace                 --                      --                      ReplaceContentsNoBackup
       // --                      --                      --                      ReplaceContentsNoBackupIgnoringMetadataErrors
       // --                      --                      --                      ReplaceContentsWithBackup
       // --                      --                      --                      ReplaceContentsWithBackupIgnoringMetadataErrors
       // SetAttributes           SetAttributes           --                      --
       // SetCreationTime         SetCreationTime         --                      --
       // SetCreationTimeUtc      (not needed)
       // SetLastAccessTime       SetLastAccessTime       --                      --
       // SetLastAccessTimeUtc    (not needed)
       // SetLastWriteTime        SetLastWriteTime        --                      --
       // SetLastWriteTimeUtc     (not needed)
       // WriteAllBytes           --                      --                      WriteAllBytes
       // WriteAllLines           --                      --                      WriteAllLines
       // WriteAllText            --                      --                      WriteAllText

       // not available in .Net Standard 2.0
       // AppendAllTextAsync
       // ReadAllBytesAsync
       // ReadAllLinesAsync
       // ReadAllTextAsync
       // WriteAllBytesAsync
       // WriteAllLinesAsync
       // WriteAllTextAsync

       // These tests document what is:  test failures means an implementation detail has changed
       // change the assertion to document "what is"
       // if you believe the behavior to be incorrect, modify the behavior of the abstraction, fix the abstraction tests, and update these documentation tests
       // to show "what is"

       // currently stripped; porting changed too much.

       [TestClass]
      public class File_BCL_Issues : TestBase
      {
          [TestMethod]
         [Ignore("Proof of bug in BCL")]
         public void It_should_but_cannot_delete_a_temporary_file_after_ReadLines()
         {
            var filePath = Path.GetTempFileName();
            try
            {
               File.ReadLines(filePath);
            }
            finally
            {
               File.Delete(filePath);
               // This throws
               // Test method ***.File_Baseline_Tests+File_BCL_Issues.I_should_but_cannot_delete_a_temporary_file_after_ReadLines threw exception: 
               // System.IO.IOException: The process cannot access the file '***\tmp59EF.tmp' because it is being used by another process.
               //   at System.IO.FileSystem.DeleteFile(String fullPath)
               //at System.IO.File.Delete(String path)
               //at ***.BclBaseline.File_Baseline_Tests.File_BCL_Issues.I_should_but_cannot_delete_a_temporary_file_after_ReadLines()...
            }
         }
      }
   }
}
