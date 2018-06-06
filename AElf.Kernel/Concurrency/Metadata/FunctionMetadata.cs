﻿using System;
using System.Collections.Generic;

namespace AElf.Kernel.Concurrency.Metadata
{
    /// <summary>
    /// The Metadata will not be changed after they are calculated as long as the related contracts don't update.
    /// Thus for each function, we store the whole set of metadata (which generated by accessing function's metadata recursively according to calling_set)
    /// When the contracts update, the metadata of related contracts' functions must be updated accordingly.
    /// </summary>
    public class FunctionMetadata
    {
        public FunctionMetadata(HashSet<string> callingSet, HashSet<Resource> fullResourceSet, HashSet<Resource> localResourceSet)
        {
            CallingSet = callingSet ?? new HashSet<string>();
            FullResourceSet = fullResourceSet ?? new HashSet<Resource>();
            LocalResourceSet = localResourceSet ?? new HashSet<Resource>();
        }
        
        /// <summary>
        /// used to find influenced contract when a contract is updated
        /// </summary>
        public HashSet<string> CallingSet { get; }

        /// <summary>
        /// used to find what resource this function will access (recursive)
        /// </summary>
        public HashSet<Resource> FullResourceSet { get; }
        
        /// <summary>
        /// used when updating a function, the caller functions of this updating function should use this NonRecursivePathSet to regenerate the new metadata
        /// </summary>
        public HashSet<Resource> LocalResourceSet { get; }
    }

    public class FunctionMetadataTemplate
    {
        public FunctionMetadataTemplate(HashSet<string> callingSet, HashSet<Resource> localResourceSet)
        {
            CallingSet = callingSet ?? new HashSet<string>();
            LocalResourceSet = localResourceSet ?? new HashSet<Resource>();
        }
        
        /// <summary>
        /// used to find influenced contract when a contract is updated
        /// </summary>
        public HashSet<string> CallingSet { get; }
        
        /// <summary>
        /// used when updating a function, the caller functions of this updating function should use this NonRecursivePathSet to regenerate the new metadata
        /// </summary>
        public HashSet<Resource> LocalResourceSet { get; }
    }

    public class Resource
    {
        public Resource(string name, DataAccessMode dataAccessMode)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DataAccessMode = dataAccessMode;
        }

        public string Name { get; }
        public DataAccessMode DataAccessMode { get; }
        
        public override int GetHashCode()
        {
            return Name.GetHashCode() + DataAccessMode.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Resource)
            {
                return this.Name == ((Resource) obj).Name && this.DataAccessMode == ((Resource) obj).DataAccessMode;
            }

            return false;
        }
    }
}