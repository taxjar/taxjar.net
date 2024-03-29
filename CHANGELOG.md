# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [4.0.0] - 2023-11-07
- Update RestSharp to version 108.0.3
- Update Newtonsoft.Json to version 13.0.3
- Update target framework to netstandard2.0, and remove net452 as a target framework

## [3.3.2] - 2021-10-20
- Update RestSharp to version 106.12.0

## [3.3.1] - 2020-11-02
- Fix async methods when passing a `TransactionId` or `CustomerId` property

## [3.3.0] - 2020-03-31
- Include custom user agent for debugging and informational purposes

## [3.2.1] - 2020-03-09
- Fix .NET standard binary version

## [3.2.0] - 2020-03-04
- Update RestSharp to version 106.10.1
- Improve performance of async methods with `ConfigureAwait(false)`

## [3.1.2] - 2020-01-15
- Update Newtonsoft.Json to version 12.0.3

## [3.1.1] - 2019-12-16
- Ignore null values passed to optional `ExemptionType` and `Provider` params for non-anonymous objects

## [3.1.0] - 2019-07-09
- Add `provider` param to transaction methods
- Support order-level exemptions via `exemption_type` param

## [3.0.2] - 2019-03-05
- Handle `null` response attributes returned from `v2/rates` [#26](https://github.com/taxjar/taxjar.net/pull/26)

## [3.0.1] - 2019-01-02
- Fix `TaxForOrder` `breakdown` response attribute deserialization [#23](https://github.com/taxjar/taxjar.net/pull/23)

## [3.0.0] - 2018-11-27
- Add an Async version of each method to support Async / Await

## [2.3.2] - 2018-11-09
- Accept `customer_id` or `CustomerId` in `UpdateCustomer` method

## [2.3.1] - 2018-10-31
- Accept `transaction_id` or `TransactionId` in `UpdateOrder` and `UpdateRefund` methods

## [2.3.0] - 2018-10-18
- Support address validation for Plus customers with `ValidateAddress` method

## [2.2.1] - 2018-10-15
- Update RestSharp dependency to `v106.5.3`

## [2.2.0] - 2018-09-18
- Add `jurisdictions` object to `TaxForOrder` response
- Support custom timeouts when instantiating client

## [2.1.0] - 2018-05-02
- Support customer exemptions

## [2.0.1] - 2018-04-05
- Improve support for domain-specific models in API requests

## [2.0.0] - 2018-04-05
- Sandbox environment support with `apiUrl` and custom headers

## [1.4.0] - 2017-11-29
- Support .NET standard 2 and .NET framework 4.5.2+

## [1.3.4] - 2017-09-22
- Support `product_tax_code` attribute in line item entity

## [1.3.3] - 2017-08-04
- Refactor `breakdown` response attribute from `TaxForOrder`

## [1.3.2] - 2017-06-06
- Support Australia / SST `country_rate` in `RatesForLocation`

## [1.3.1] - 2017-03-02
- Deserialize null values correctly in `DeleteOrder` and `DeleteRefund` responses [#1](https://github.com/taxjar/taxjar.net/pull/1)

## [1.3.0] - 2017-01-30
- Fix type of `Id` (`string`) in LineItem and TaxBreakdownLineItem

## [1.2.0] - 2017-01-19
- Accept type `string` as transaction ID param in show/delete methods

## [1.1.0] - 2016-11-03
- Support all /v2/taxes breakdown attributes including Canada and EU

## [1.0.2] - 2016-11-03
- Fix exception handling for `201` responses

## [1.0.1] - 2016-10-04
- Improve exception handling, show message for ApplicationException

## [1.0.0] - 2016-09-20
- Initial release

[Unreleased]: https://github.com/taxjar/taxjar.net/compare/v4.0.0...HEAD
[4.0.0]: https://github.com/taxjar/taxjar.net/compare/v3.3.2...v4.0.0
[3.3.2]: https://github.com/taxjar/taxjar.net/compare/v3.3.1...v3.3.2
[3.3.1]: https://github.com/taxjar/taxjar.net/compare/v3.3.0...v3.3.1
[3.3.0]: https://github.com/taxjar/taxjar.net/compare/v3.2.1...v3.3.0
[3.2.1]: https://github.com/taxjar/taxjar.net/compare/v3.2.0...v3.2.1
[3.2.0]: https://github.com/taxjar/taxjar.net/compare/v3.1.2...v3.2.0
[3.1.2]: https://github.com/taxjar/taxjar.net/compare/v3.1.1...v3.1.2
[3.1.1]: https://github.com/taxjar/taxjar.net/compare/v3.1.0...v3.1.1
[3.1.0]: https://github.com/taxjar/taxjar.net/compare/v3.0.2...v3.1.0
[3.0.2]: https://github.com/taxjar/taxjar.net/compare/v3.0.1...v3.0.2
[3.0.1]: https://github.com/taxjar/taxjar.net/compare/v3.0.0...v3.0.1
[3.0.0]: https://github.com/taxjar/taxjar.net/compare/v2.3.2...v3.0.0
[2.3.2]: https://github.com/taxjar/taxjar.net/compare/v2.3.1...v2.3.2
[2.3.1]: https://github.com/taxjar/taxjar.net/compare/v2.3.0...v2.3.1
[2.3.0]: https://github.com/taxjar/taxjar.net/compare/v2.2.1...v2.3.0
[2.2.1]: https://github.com/taxjar/taxjar.net/compare/v2.2.0...v2.2.1
[2.2.0]: https://github.com/taxjar/taxjar.net/compare/v2.1.0...v2.2.0
[2.1.0]: https://github.com/taxjar/taxjar.net/compare/v2.0.1...v2.1.0
[2.0.1]: https://github.com/taxjar/taxjar.net/compare/v2.0.0...v2.0.1
[2.0.0]: https://github.com/taxjar/taxjar.net/compare/v1.4.0...v2.0.0
[1.4.0]: https://github.com/taxjar/taxjar.net/compare/v1.3.4...v1.4.0
[1.3.4]: https://github.com/taxjar/taxjar.net/compare/v1.3.3...v1.3.4
[1.3.3]: https://github.com/taxjar/taxjar.net/compare/v1.3.2...v1.3.3
[1.3.2]: https://github.com/taxjar/taxjar.net/compare/v1.3.1...v1.3.2
[1.3.1]: https://github.com/taxjar/taxjar.net/compare/v1.3.0...v1.3.1
[1.3.0]: https://github.com/taxjar/taxjar.net/compare/v1.2.0...v1.3.0
[1.2.0]: https://github.com/taxjar/taxjar.net/compare/v1.1.0...v1.2.0
[1.1.0]: https://github.com/taxjar/taxjar.net/compare/v1.0.2...v1.1.0
[1.0.2]: https://github.com/taxjar/taxjar.net/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/taxjar/taxjar.net/compare/v1.0.0...v1.0.1
