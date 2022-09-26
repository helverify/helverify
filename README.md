# Helverify - Verifiable Remote Postal Voting

Helverify is the prototype of a verifiable Remote Postal Voting (RPV) system for use in Switzerland. This repository contains the code of Helverify's components.

# Prerequisites
This section lists the prerequisites of running Helverify on your computer.

## Hardware Requirements
Helverify has been evaluated with the following hardware:

- CPU: AMD Ryzen 9 5950X (16C/32T)
- RAM: 64 GB
- Storage: 1 TB of NVME SSD storage

Note, that for small number of ballots and minimum key lengths, Helverify might also run on less powerful computers, but in general, the more resources your computer has, the better. This is primarily due to the cryptographic operations (CPU) and the blockchain (RAM), which are demanding in terms of computational resources.

## Software Requirements
To run Helverify, you need to set up the following environment:
- [Ubuntu 22.04 LTS](https://ubuntu.com/download/desktop) (highly recommended)
- [Docker](https://www.docker.com/)
- Docker Compose
- [MongoDB Compass](https://www.mongodb.com/products/compass) (optional)

With these tools installed, you are ready to go.

If you still want to try out Helverify on Windows, make the following adjustments to docker/docker-compose.yml:

```yaml
	...
	va-backend:
		...
		environment:
			...
			- IpfsHost=http://host.docker.internal:5001 # instead of http://172.17.0.1:5001
			...
		...
	consensus-node1:
		...
		environment:
			...
			- IpfsHost=http://host.docker.internal:5001 # instead of http://172.17.0.1:5001
			...
		...
	consensus-node2:
		...
		environment:
			...
			- IpfsHost=http://host.docker.internal:5001 # instead of http://172.17.0.1:5001
			...
		...
	consensus-node3:
		...
		environment:
			...
			- IpfsHost=http://host.docker.internal:5001 # instead of http://172.17.0.1:5001
			...
		...
	...
	ipfs-node1:
		...
		ports:
			...
			- 5001:5001 # instead of "172.17.0.1:5001:5001"
			...
	...
```

# Getting Started
Getting Helverify up and running is quite straightforward: Using the following commands, an instance of Helverify with one Voting Authority, three Consensus Nodes, three IPFS nodes in a private swarm, a Voter frontend, and a MongoDB are launched:
```bash
cd docker
docker compose up
```
## Applications
By default, Helverify allocates the following Ports to the respective applications:

- [Voting Authority Frontend](http://localhost:3000): 3000
- [Voting Authority Backend](http://localhost:5000): 5000
  - SwaggerUI: http://localhost:5000/swagger/index.html
- [Voter Frontend](http://localhost:3001): 3001
- [Consensus Node 1](http://localhost:5002): 5002
  - Swagger UI: http://localhost:5002/swagger/index.html
- [Consensus Node 2](http://localhost:5003): 5003
  - Swagger UI: http://localhost:5003/swagger/index.html
- [Consensus Node 3](http://localhost:5004): 5004
  - Swagger UI: http://localhost:5004/swagger/index.html

Further port allocations and services can be found in the [docker-compose.yml](docker/docker-compose.yml) file.

# Development
If you want to change the implementation, there are some things to consider before doing so.

## Technology
For the development of Helverify, the following technologies were used:

- [C# ASP.NET 6](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-6.0?view=aspnetcore-6.0) for the Voting Authority and Consensus Node backends
- [React](https://reactjs.org/) with [TypeScript](https://www.typescriptlang.org/) for the Voting Authority and Voter frontends
- [MongoDB](https://www.mongodb.com/) for the Voting Authority database
- [Solidity](https://docs.soliditylang.org/en/v0.8.17/) for the Election Smart Contract
- [Go-Ethereum](https://geth.ethereum.org/) (geth) for running blockchain sealers inside the Consensus Nodes
- [IPFS](https://ipfs.tech/) for storing encryptions and evidence

## Cryptography Library
The cryptography library for C# is located at the `cryptography` folder. Check out the [dedicated README](cryptography/dotnet/Helverify.Cryptography/README.md) for further details.

If you decide to make changes to the cryptography library, consider the following things:

As there is not package manager used in this project (for C#), you have to update the dependent projects (*Voting Authority Backend* and *Consensus Node Backend*) by executing the following scripts:
```bash 
cd scripts
./build-helverify-cryptography.sh
./update-cryptography.sh
```
This builds the cryptography library and copies the resulting DLL library to the correct locations in the two said projects. In the projects themselves, there is nothing to do to apply the changes apart from rebuilding.

## Scripts
Helverify uses a series of scripts to manage the state of the blockchain clients ([Go-Ethereum](https://geth.ethereum.org/)). These scripts are located in the `scripts/blockchain` folder, which is where changes to these scripts must be applied for the changes to be persistent. After changing the scripts, run the following script:
```bash
cd scripts
./update-bc-scripts.sh
```
This makes sure that both the Voting Authority and Consensus Node backends use the updated scripts for building new docker images.
