
# Helverify.Cryptography
This library provides a C# implementation (NET 6.0) of the cryptographic primitives used in Helverify. It is based on the [BouncyCastle](https://www.bouncycastle.org/) library.

## Crypographic Primitives
The cryptographic primitives contained in this library is the following:
 - Standard ElGamal cryptosystem
	 - Including Distributed Key Generation
	 - Including Cooperative Decryption
 - Exponential ElGamal cryptosystem
	 - Including Distributed Key Generation
	 - Including Cooperative Decryption
 - Non-Interactive Chaum-Pedersen Zero-Knowledge Proofs
	 - Proof of correct decryption of an ElGamal ciphertext
	 - Proof of a ciphertext containing the value "1"
 - Non-Interactive Disjunctive Chaum-Pedersen Zero-Knowlege Proof
	 - Proof of a ciphertext containing either the value "0" or "1"
 - Non-Interactive Schnorr Zero-Knowlege Proof
	 - Proof of owning a private key to a specific public key

## Usage
### Example: ElGamal Encrypt and Decrypt

```csharp
IElGamal elGamal = new ExponentialElGamal();

// choose a predefined Diffie-Hellman group
DhGroup dhGroup = DhGroups.Get(DhGroups.Modp2048);

BigInteger p = dhGroup.P;
BigInteger g = dhGroup.G;

// generate a key pair
AsymmetricCipherKeyPair keyPair = elGamal.KeyGen(p, g);

int message = 1;

// encrypt the message using the public key
ElGamalCipher cipher = elGamal.Encrypt(message, keyPair.Public);
 
// decrypt the ciphertext using the private key
int decryptedMessage = elGamal.Decrypt(cipher, keyPair.Private);
```

### Example: ElGamal Distributed Key Generation / Cooperative Decryption
```csharp
IElGamal elGamal = new ExponentialElGamal();

// choose a predefined Diffie-Hellman group
DhGroup dhGroup = DhGroups.Get(DhGroups.Modp2048);

BigInteger p = dhGroup.P;
BigInteger g = dhGroup.G;

// generate multiple key pairs
AsymmetricCipherKeyPair keyPair1 = elGamal.KeyGen(p, g);
AsymmetricCipherKeyPair keyPair2 = elGamal.KeyGen(p, g);
AsymmetricCipherKeyPair keyPair3 = elGamal.KeyGen(p, g);

// combine the public keys into one composite public key
DHPublicKeyParameters combinedPublicKey = elGamal.CombinePublicKeys(new List<DHPublicKeyParameters>
{
	(keyPair1.Public as DHPublicKeyParameters)!,
	(keyPair2.Public as DHPublicKeyParameters)!,
	(keyPair3.Public as DHPublicKeyParameters)!
}, elGamal.GetParameters(p, g));

int message = 1;

// encrypt the message with the composite public key
ElGamalCipher cipher = elGamal.Encrypt(message, combinedPublicKey);

// decrypt the shares using the three private keys
BigInteger share1 = elGamal.DecryptShare(cipher, keyPair1.Private as DHPrivateKeyParameters, p);
BigInteger share2 = elGamal.DecryptShare(cipher, keyPair2.Private as DHPrivateKeyParameters, p);
BigInteger share3 = elGamal.DecryptShare(cipher, keyPair3.Private as DHPrivateKeyParameters, p);

// combine the results to retrieve the decrypted message
int decryptedMessage = elGamal.CombineShares(new List<BigInteger> { share1, share2, share3 }, cipher.D, p, g);
```

### Example: ElGamal Homomorphic Addition
```csharp
IElGamal elGamal = new ExponentialElGamal();

// choose a predefined Diffie-Hellman group
DhGroup dhGroup = DhGroups.Get(DhGroups.Modp2048);

BigInteger p = dhGroup.P;
BigInteger g = dhGroup.G;

// generate a key pair
AsymmetricCipherKeyPair keyPair = elGamal.KeyGen(p, g);

int message1 = 1;
int message2 = 1;

// encrypt the messages using the public key
ElGamalCipher cipher1 = elGamal.Encrypt(message1, keyPair.Public);
ElGamalCipher cipher2 = elGamal.Encrypt(message2, keyPair.Public);

// perform the addition of the ciphertexts
ElGamalCipher combinedCipher = cipher1.Add(cipher1, p);
```

# Sources
To build this library, the following sources were used:

 - [1] David Bernhard and Bogdan Warinschi. Cryptographic Voting - A Gentle Introduction, 2016.  https://eprint.iacr.org/2016/765.pdf  accessed 13 May 2022.
 - [2] David Chaum and Torben Pryds Pedersen.  Wallet databases with observers.  In Ernest F. Brickell, editor,  Advances in Cryptology — CRYPTO’ 92, pages 89–105, Berlin, Heidelberg, 1993. Springer Berlin Heidelberg.
 - [3] Ronald Cramer, Ivan Damgård, and Berry Schoenmakers. Proofs of partial knowledge and simplified design of witness hiding protocols. In Yvo G. Desmedt, editor, Advances in Cryptology — CRYPTO ’94, pages 174–187, Berlin, Heidelberg, 1994. Springer Berlin Heidelberg.
 - [4] Taher Elgamal. A public key cryptosystem and a signature scheme based on discrete logarithms.  IEEE Transactions on Information Theory, 31(4):469–472, 1985.
 - [5] Amos Fiat and Adi Shamir. How to prove yourself: Practical solutions to identification and signature problems. In Andrew M. Odlyzko, editor,  Advances in Cryptology — CRYPTO’ 86, pages 186–194, Berlin, Heidelberg, 1987. Springer Berlin Heidelberg.
- [6] Christian Killer, Bruno Rodrigues, Eder John Scheid, Muriel Franco, Moritz Eck, Nik Zaugg, Alex Scheitlin, and Burkhard Stiller. Provotum: A blockchain-based and end-to-end verifiable remote electronic voting system. In  2020 IEEE 45th Conference on Local Computer Networks (LCN), pages 172–183, 2020.
 - [7] C. P. Schnorr. Efficient signature generation by smart cards.  Journal of Cryptology, 4(3):161–174, January 1991.

Additionally, this library has been inspired by the following projects:

 - https://github.com/meck93/evote-crypto
 - https://github.com/meck93/evote-crypto/blob/master/src/ff-elGamal/encryption.ts
 - https://github.com/bcgit/bc-csharp/
 - https://nvotes.com/multiplicative-vs-additive-homomorphic-elGamal/

