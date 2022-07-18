import {BigNumberHelper} from "../helper/bigNumberHelper";
import bigInt from "big-integer";
import {ElGamal} from "../cryptography/elGamal";
import {Cipher} from "../ballot/encryptedBallot";

describe("ElGamal tests", () => {
    it("should produce the same encryption as c#", () =>{
        // arrange
        const expectedC: bigInt.BigInteger = BigNumberHelper.fromHexString("39dc122d380db2b6ae980352f38e8668c1fc0ccc5a13a334ccf9e162e32c5c614b14ea8ace46d809288fb4bf55ee6a1899b97692294d72f34305c9276c9baa68ea3c514912c6a7f04611f51089821b215be7db15f50e8b398fd3e25ec4dd1763a54935f5273ca24ad6ac05354eab30e481248b282d7c1bf3ff757b4ea9868db98a74efd042898838d0770da9f623a40531f22249d6f72af868012ce2ccef2127ef694e494b729d2ae15e1cd96f1704aafb48287431fede4d6d343f3f19933f34d3261979c63d7f8c45b4d21bfff1556bb87b829d5ceb018a95e475b71ec5c2748bb2f38131a6f3780939afac2150e25780568237435484ed5c0ceed890323e36");
        const expectedD: bigInt.BigInteger = BigNumberHelper.fromHexString("a99624af8cf1fad40fcd5b6e01956ff9375a4abf30e57d07641be3047588cc54614741eef56aed5c499750aaf00f3010c645179283aff82fac828433ff75bb007e0f4de542e80c2ec9308dd89517fa1ca9fbe692454f2ba442d77d2f072d136c8d5f48493a0757387e98db8dc3c0ae2ba1e5bd8577d44a116de9df9088fff17807353b4a0b5c9421577b338ba7f143d45e5e69a39a0bbb0bcaa6edf48202f7bc0cb434eac689d9d9b6f0b96c8521dfdcf67f6e31f94933e6e72d27feeeabaa1384b2390de7347fbf192ce870c8d153a1b06ec049ebbe4fe026f4d55e515d9dd889172d311e041b4feba889d18aa8791a8fe4a87805a251a82da154a4b36ca19");

        const elGamal: ElGamal = new ElGamal();

        const p: bigInt.BigInteger = BigNumberHelper.fromHexString("ffffffffffffffffc90fdaa22168c234c4c6628b80dc1cd129024e088a67cc74020bbea63b139b22514a08798e3404ddef9519b3cd3a431b302b0a6df25f14374fe1356d6d51c245e485b576625e7ec6f44c42e9a637ed6b0bff5cb6f406b7edee386bfb5a899fa5ae9f24117c4b1fe649286651ece45b3dc2007cb8a163bf0598da48361c55d39a69163fa8fd24cf5f83655d23dca3ad961c62f356208552bb9ed529077096966d670c354e4abc9804f1746c08ca18217c32905e462e36ce3be39e772c180e86039b2783a2ec07a28fb5c55df06f4c52c9de2bcbf6955817183995497cea956ae515d2261898fa051015728e5a8aacaa68ffffffffffffffff");
        const g: bigInt.BigInteger = BigNumberHelper.fromHexString("2");
        const publicKey: bigInt.BigInteger = BigNumberHelper.fromHexString("505a3e9a453ca5177325acfab3fb1e5e4c6090c7395e995856b599dd6197509873dee31a57eb667a287ea650d4acdd65c15775268134f8e0897aeee3df83e57e27d7d84be71a25e441d4ebcad39684869a4c19abe62fd05c549dbf7de7f2a920addbb4bb4143badc29fe71f0724aa1f6a2550324e16bc70fbcbdfc8eb0bce28273fc615ba907e521ad8d8ae57cc4f54d2f84f31e15bc4be53095604b6403febcffd4b79453da2d4aee8f7685728f5472fa33484acb878be6cfe450c7ae44bf600dd8d611d2ce79ae14f040661a8b43e2fd5791e47a9452e0565bf9e3d027df97bf7e3209422014196170252ee55bf349d2230754537ddde0bbd8f446159c3cf6");

        const randomness: bigInt.BigInteger = BigNumberHelper.fromHexString("d564112f");

        // act
        const cipher: Cipher = elGamal.encrypt(bigInt(1), publicKey, p, g, randomness);

        // assert
        expect(cipher.c.toString(16)).toStrictEqual(expectedC.toString(16));
        expect(cipher.d.toString(16)).toStrictEqual(expectedD.toString(16));
    });
})