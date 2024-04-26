global using Xunit;

// Tests must run sequentially to avoid interfering with one another
[assembly: CollectionBehavior(DisableTestParallelization = true)]