﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    public class GenericParameter : IType
    {
        private readonly string _declarerFullName;
        internal readonly IEnumerable<ITypeInstance<IType>> TypeInstanceConstraints;

        public GenericParameter(
            string declarerFullName,
            string name,
            GenericParameterVariance variance,
            IEnumerable<ITypeInstance<IType>> typeConstraints,
            bool hasReferenceTypeConstraint,
            bool hasNotNullableValueTypeConstraint,
            bool hasDefaultConstructorConstraint,
            bool isCompilerGenerated,
            bool declarerIsMethod
        )
        {
            _declarerFullName = declarerFullName;
            Name = name;
            Variance = variance;
            TypeInstanceConstraints = typeConstraints;
            HasReferenceTypeConstraint = hasReferenceTypeConstraint;
            HasNotNullableValueTypeConstraint = hasNotNullableValueTypeConstraint;
            HasDefaultConstructorConstraint = hasDefaultConstructorConstraint;
            IsCompilerGenerated = isCompilerGenerated;
            DeclarerIsMethod = declarerIsMethod;
        }

        public IType DeclaringType { get; private set; }

        [CanBeNull]
        public IMember DeclaringMethod { get; private set; }
        public bool DeclarerIsMethod { get; }
        public GenericParameterVariance Variance { get; }
        public IEnumerable<IType> TypeConstraints =>
            TypeInstanceConstraints.Select(instance => instance.Type);
        public bool HasReferenceTypeConstraint { get; }
        public bool HasNotNullableValueTypeConstraint { get; }
        public bool HasDefaultConstructorConstraint { get; }

        public bool HasConstraints =>
            HasReferenceTypeConstraint
            || HasNotNullableValueTypeConstraint
            || HasDefaultConstructorConstraint
            || TypeConstraints.Any();

        public string Name { get; }
        public string FullName => _declarerFullName + "+<" + Name + ">";
        public string AssemblyQualifiedName =>
            System.Reflection.Assembly.CreateQualifiedName(
                DeclaringType.Assembly.FullName,
                FullName
            );
        public bool IsCompilerGenerated { get; }
        public IEnumerable<Attribute> Attributes =>
            AttributeInstances.Select(instance => instance.Type);
        public List<AttributeInstance> AttributeInstances { get; } = new List<AttributeInstance>();

        public List<ITypeDependency> Dependencies { get; } = new List<ITypeDependency>();
        public List<ITypeDependency> BackwardsDependencies { get; } = new List<ITypeDependency>();
        public Visibility Visibility => Visibility.NotAccessible;
        public bool IsGeneric => false;
        public bool IsGenericParameter => true;
        public List<GenericParameter> GenericParameters => new List<GenericParameter>();
        public Namespace Namespace => DeclaringType?.Namespace;
        public Assembly Assembly => DeclaringType?.Assembly;
        public MemberList Members => new MemberList();
        public IEnumerable<IType> ImplementedInterfaces => Enumerable.Empty<IType>();

        public bool IsNested => true;
        public bool IsStub => true;

        internal void AssignDeclarer(IMember declaringMethod)
        {
            if (!declaringMethod.FullName.Equals(_declarerFullName))
            {
                throw new InvalidOperationException("Full name of declaring member doesn't match.");
            }

            DeclaringType = declaringMethod.DeclaringType;
            DeclaringMethod = declaringMethod;
        }

        internal void AssignDeclarer(IType declaringType)
        {
            if (!declaringType.FullName.Equals(_declarerFullName))
            {
                throw new InvalidOperationException("Full name of declaring type doesn't match.");
            }

            DeclaringType = declaringType;
        }

        public bool Equals(GenericParameter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(FullName, other.FullName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((GenericParameter)obj);
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public override string ToString()
        {
            return FullName;
        }
    }

    public enum GenericParameterVariance
    {
        NonVariant,
        Covariant,
        Contravariant,
    }
}
