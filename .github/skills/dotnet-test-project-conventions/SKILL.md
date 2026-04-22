---
name: dotnet-test-project-conventions
description: 'Create or review .NET 10 xUnit tests using this project style. Use for unit tests, test refactors, mocking with NSubstitute, AutoFixture-based test data, BaseTest inheritance, Arrange-Act-Assert structure, Given-When-Then naming, and fast isolated test design.'
argument-hint: 'What code or behavior should be tested?'
---

# .NET Test Project Conventions

## What This Skill Produces

This skill helps the agent create, update, or review unit tests that match the project's current test conventions:

- xUnit on .NET 10 with C# 14
- Test classes inheriting from `BaseTest`
- NSubstitute for mocking dependencies
- AutoFixture for generating test data and creating objects
- `FreezeSubstitute` for registering and reusing mocked dependencies
- Arrange-Act-Assert test structure
- Given-When-Then test naming
- Independent, isolated, fast-running tests

## When to Use

Use this skill when the task involves:

- adding unit tests for application or domain code
- updating tests after production code changes
- refactoring tests to match existing team conventions
- reviewing whether tests follow project standards
- introducing mocks, test fixtures, builders, or custom assertions

Do not use this skill for:

- end-to-end or browser-based tests
- load or performance testing
- integration tests
- any test that depends on infrastructure, process boundaries, or shared runtime state

## Procedure

1. Identify the testing target.
   - Confirm the class, handler, endpoint, or behavior being tested.
   - Prefer the narrowest observable behavior instead of implementation-only details.

2. Choose the test shape.
   - This skill is for unit tests only.
   - Dependencies should be isolated with substitutes or simple real objects.
   - Reuse `BaseTest` for common setup and helpers.
   - Prefer one assertion concept per test, even if that concept uses multiple related assertions.

3. Set up dependencies.
   - Mock collaborators with NSubstitute.
   - Use AutoFixture to generate data and reduce manual object construction.
   - Use `FreezeSubstitute` when a dependency must be consistently injected and inspected across the test.
   - Use interface-based mocking for services.

4. Prepare test data.
   - Prefer reusable fixtures for repeated domain data.
   - Use builder patterns when objects are complex or require intent-revealing setup.
   - Keep setup minimal and specific to the scenario.

5. Write the test using Arrange-Act-Assert.
   - Arrange only the state and inputs required for the behavior.
   - Act with a single focused invocation.
   - Assert the expected outcome with clear, descriptive checks.
   - Use custom assertion helpers for domain-specific validation when they improve readability.

6. Name the test with Given-When-Then intent.
   - Example: `GivenInvalidStatus_WhenCompletingWorkItem_ThenThrowsValidationException`
   - Keep names behavior-focused rather than implementation-focused.

7. Verify isolation and speed.
   - The test should not depend on external state, execution order, or shared mutable data.
   - Avoid unnecessary waits, file I/O, network access, or database usage in unit tests.

## Decision Points

### Mock or Real Instance?

- Use a substitute when the dependency is external to the behavior under test or needs interaction verification.
- Use a real object when it is simple, deterministic, and part of the behavior you actually want to validate.

### AutoFixture or Builder?

- Use AutoFixture for straightforward objects or random-but-valid inputs.
- Use a builder when the object has important semantic states that should be obvious in the test.

### Generic Assertion or Custom Helper?

- Use direct assertions when the expected outcome is simple and obvious.
- Use a custom assertion helper when domain validation would otherwise make tests repetitive or harder to read.

### Single Test or Split Tests?

- Keep one assertion concept per test.
- Split tests when multiple outcomes represent different behaviors, branches, or failure reasons.

## Completion Checks

A test produced with this skill is complete when:

- the test class inherits from `BaseTest`
- the test name follows Given-When-Then style
- the structure is clearly Arrange-Act-Assert
- dependencies are isolated with NSubstitute or replaced with simple real objects intentionally
- test data creation uses AutoFixture, fixtures, or builders appropriately
- assertions are descriptive and validate the intended behavior
- the test is independent and fast to run

## Review Checklist

Use this checklist when reviewing an existing test:

- Does the test cover behavior rather than internal implementation details?
- Does the class inherit from `BaseTest`?
- Are mocks created with NSubstitute where appropriate?
- Is AutoFixture or a builder used instead of noisy manual setup?
- Is there one clear assertion concept?
- Is the test name descriptive and in Given-When-Then form?
- Is the test isolated from external systems and shared state?

## Expected Inputs

Provide one or more of these when invoking the skill:

- target class or method to test
- expected behavior or failure mode
- existing production code path
- existing test file to refactor

## Expected Output

The skill should produce one of the following:

- a new test class or test methods matching project conventions
- a refactored test aligned with the project style
- a review of convention gaps and concrete fixes needed
