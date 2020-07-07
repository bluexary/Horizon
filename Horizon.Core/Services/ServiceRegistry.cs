﻿using Horizon.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Horizon.Core.Services
{
    public class ServiceRegistry : IServiceInstances, IServiceHealthCheck, IResolveServiceInstances, IHaveKeyValues
    {


        private readonly IRegistryHost _registryHost;
        private IResolveServiceInstances _serviceInstancesResolver;

        public ServiceRegistry(IRegistryHost registryHost)
        {
            _registryHost = registryHost;
        }

        public async Task<ServiceInformation> RegisterServiceAsync(string serviceName, string version, Uri uri, Uri healthCheckUri = null, IEnumerable<string> tags = null)
        {
            var registryInformation = await _registryHost.RegisterServiceAsync(serviceName, version, uri, healthCheckUri, tags);

            return registryInformation;
        }

        public async Task<bool> DeregisterServiceAsync(string serviceId)
        {
            return await _registryHost.DeregisterServiceAsync(serviceId);
        }

        public async Task<string> RegisterHealthCheckAsync(string serviceName, string serviceId, Uri checkUri, TimeSpan? interval = null, string notes = null)
        {
            return await _registryHost.RegisterHealthCheckAsync(serviceName, serviceId, checkUri, interval, notes);
        }

        public async Task<bool> DeregisterHealthCheckAsync(string checkId)
        {
            return await _registryHost.DeregisterHealthCheckAsync(checkId);
        }

        public async Task<IList<ServiceInformation>> FindServiceInstancesAsync()
        {
            return _serviceInstancesResolver == null
                ? await _registryHost.FindServiceInstancesAsync()
                : await _serviceInstancesResolver.FindServiceInstancesAsync();
        }

        public async Task<IList<ServiceInformation>> FindServiceInstancesAsync(string name)
        {
            return _serviceInstancesResolver == null
                ? await _registryHost.FindServiceInstancesAsync(name)
                : await _serviceInstancesResolver.FindServiceInstancesAsync(name);
        }

        public async Task<IList<ServiceInformation>> FindServiceInstancesWithVersionAsync(string name, string version)
        {
            return _serviceInstancesResolver == null
                ? await _registryHost.FindServiceInstancesWithVersionAsync(name, version)
                : await _serviceInstancesResolver.FindServiceInstancesWithVersionAsync(name, version);
        }

        public async Task<IList<ServiceInformation>> FindServiceInstancesAsync(Predicate<KeyValuePair<string, string[]>> nameTagsPredicate,
            Predicate<ServiceInformation> ServiceInformationPredicate)
        {
            return _serviceInstancesResolver == null
                ? await _registryHost.FindServiceInstancesAsync(nameTagsPredicate, ServiceInformationPredicate)
                : await _serviceInstancesResolver.FindServiceInstancesAsync(nameTagsPredicate, ServiceInformationPredicate);
        }

        public async Task<IList<ServiceInformation>> FindServiceInstancesAsync(Predicate<KeyValuePair<string, string[]>> predicate)
        {
            return _serviceInstancesResolver == null
                ? await _registryHost.FindServiceInstancesAsync(predicate)
                : await _serviceInstancesResolver.FindServiceInstancesAsync(predicate);
        }

        public async Task<IList<ServiceInformation>> FindServiceInstancesAsync(Predicate<ServiceInformation> predicate)
        {
            return _serviceInstancesResolver == null
                ? await _registryHost.FindServiceInstancesAsync(predicate)
                : await _serviceInstancesResolver.FindServiceInstancesAsync(predicate);
        }

        public async Task<IList<ServiceInformation>> FindAllServicesAsync()
        {
            return _serviceInstancesResolver == null
                ? await _registryHost.FindAllServicesAsync()
                : await _serviceInstancesResolver.FindAllServicesAsync();
        }

        public async Task<ServiceInformation> AddWebapiAsync(IRegistryWebAPI registryWebapi, string serviceName, string version, Uri healthCheckUri = null, IEnumerable<string> tags = null)
        {
            var uri = registryWebapi.Uri;
            return await RegisterServiceAsync(serviceName, version, uri, healthCheckUri, tags);
        }

        public async Task<string> AddHealthCheckAsync(string serviceName, string serviceId, Uri checkUri, TimeSpan? interval = null, string notes = null)
        {
            return await RegisterHealthCheckAsync(serviceName, serviceId, checkUri, interval, notes);
        }

        public async Task KeyValuePutAsync(string key, string value)
        {
            await _registryHost.KeyValuePutAsync(key, value);
        }

        public async Task<string> KeyValueGetAsync(string key)
        {
            return await _registryHost.KeyValueGetAsync(key);
        }

        public async Task KeyValueDeleteAsync(string key)
        {
            await _registryHost.KeyValueDeleteAsync(key);
        }

        public async Task KeyValueDeleteTreeAsync(string prefix)
        {
            await _registryHost.KeyValueDeleteTreeAsync(prefix);
        }

        public async Task<string[]> KeyValuesGetKeysAsync(string prefix)
        {
            return await _registryHost.KeyValuesGetKeysAsync(prefix);
        }

        public void ResolveServiceInstancesWith<T>(T serviceInstancesResolver)
            where T : IResolveServiceInstances
        {
            _serviceInstancesResolver = serviceInstancesResolver;
        }
    }
}
