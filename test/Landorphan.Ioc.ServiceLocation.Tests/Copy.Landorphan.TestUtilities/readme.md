## Design Decision
   Landorphan.TestUtilities will be repackaged to take a dependency on both Landorphan.Common and Landorphan.Ioc, making it unavailable for test projects
   that do not share both dependencies.

   Some of the classes defined in Landorphan.TestUtilities are copied here, and are manually maintained in parallel with Landorphan.TestUtilities

