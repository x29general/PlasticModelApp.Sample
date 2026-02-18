using System;

// Backward-compatibility exception types expected by unit tests
// These types are thin wrappers that preserve the names the tests reference.

namespace PaintExceptions
{
    public class PaintNotFoundException : Exception
    {
        public PaintNotFoundException(string id)
            : base($"Paint not found: {id}")
        {
        }
    }

    public class PaintAlreadyDeletedException : Exception
    {
        public PaintAlreadyDeletedException(string id)
            : base($"Paint already deleted: {id}")
        {
        }
    }

    public class PaintAlreadyExistsException : Exception
    {
        public PaintAlreadyExistsException(string id)
            : base($"Paint already exists: {id}")
        {
        }
    }
}

namespace TagExceptions
{
    public class TagNotFoundException : Exception
    {
        public TagNotFoundException(string id)
            : base($"Tag not found: {id}")
        {
        }
    }

    public class TagAlreadyExistsException : Exception
    {
        public TagAlreadyExistsException(string id)
            : base($"Tag already exists: {id}")
        {
        }
    }
}

// ResourceNotFoundException used by some tests as unqualified type name
public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string resource, string message)
        : base($"{resource}: {message}")
    {
    }
}
