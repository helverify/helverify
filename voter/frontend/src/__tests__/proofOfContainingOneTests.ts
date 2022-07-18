import {Cipher} from "../ballot/encryptedBallot";
import {BigNumberHelper} from "../helper/bigNumberHelper";
import {ProofOfContainingOne} from "../cryptography/proofOfContainingOne";
import bigInt from "big-integer";

describe("ProofOfContainingOne test", () => {
    it("should classify proof as valid", () => {
        // arrange
        const p: bigInt.BigInteger = BigNumberHelper.fromHexString("ffffffffffffffffc90fdaa22168c234c4c6628b80dc1cd129024e088a67cc74020bbea63b139b22514a08798e3404ddef9519b3cd3a431b302b0a6df25f14374fe1356d6d51c245e485b576625e7ec6f44c42e9a637ed6b0bff5cb6f406b7edee386bfb5a899fa5ae9f24117c4b1fe649286651ece45b3dc2007cb8a163bf0598da48361c55d39a69163fa8fd24cf5f83655d23dca3ad961c62f356208552bb9ed529077096966d670c354e4abc9804f1746c08ca18217c32905e462e36ce3be39e772c180e86039b2783a2ec07a28fb5c55df06f4c52c9de2bcbf6955817183995497cea956ae515d2261898fa051015728e5a8aacaa68ffffffffffffffff");
        const g: bigInt.BigInteger = BigNumberHelper.fromHexString("2");
        const publicKey: bigInt.BigInteger = BigNumberHelper.fromHexString("505a3e9a453ca5177325acfab3fb1e5e4c6090c7395e995856b599dd6197509873dee31a57eb667a287ea650d4acdd65c15775268134f8e0897aeee3df83e57e27d7d84be71a25e441d4ebcad39684869a4c19abe62fd05c549dbf7de7f2a920addbb4bb4143badc29fe71f0724aa1f6a2550324e16bc70fbcbdfc8eb0bce28273fc615ba907e521ad8d8ae57cc4f54d2f84f31e15bc4be53095604b6403febcffd4b79453da2d4aee8f7685728f5472fa33484acb878be6cfe450c7ae44bf600dd8d611d2ce79ae14f040661a8b43e2fd5791e47a9452e0565bf9e3d027df97bf7e3209422014196170252ee55bf349d2230754537ddde0bbd8f446159c3cf6");

        const cipher: Cipher = {
            c: BigNumberHelper.fromHexString("a83edddd92a33753b8f465ddab0a27f3382577e1c76237e9e77fe5755aa4c59f157c207afd40fb6a4cf7fad9b0d67a3b905cbb0da5fe8b5758a0bd3898a05a409f3c4cb2371bf59722f6ede3e365792bab05cb09dc5533d1141f693e482f5f8619a36e87f354331255f3a0bef8cfa5619eebb2d4abbf5c3c2ea47906c965a04409602b3d783be7b35b5b073208e22d1b07ceffc8fdc238bdda4f37d121e58bb1be3edf3407631395c9e7503d9887a2daefab9ff454e9347adddc516473ede26f7b2d5eb16923695c5ae8afc217607a8657774d9d40a32b3147bbdf79775c1e334c7bd1f8fce3be7359580a6ffd362af2300dbb3e52b0cdc6c60be51768868fc2"),
            d: BigNumberHelper.fromHexString("39541d213c57572a4acdfeba77674c92935ceb500143fca83ae8d42568c755b86eaa8d91c881b6ef33ab5c3d888d2a984929304a62646c84cb588fe6589049e202aa4ce8665bbff282948e760052bca5ca322c1b6392ddd48eebeac6915e97e719694f426c83cc23fd4435547c0637038c3677fb9174852643145e47cd29fb8724a5c4f1a92a0be2cab716c26c7b69579352778e6b8c7dc649b88d53b7f98c944ba81585201cb9f8c89339db5a2e3b9ea95658402b1f66cbd33e160fbfde13ee69a4b49c2ecea4e4fe57aa709af6326bf547d7f8a4c3d45a675dff27ad635158e360e33902bda24a42dedfa30c93c139d2cc93a50fc9e7885f6e16bee32adaaf")
        }

        const proof: ProofOfContainingOne = new ProofOfContainingOne(
            "3be6d1a56940d5892cddcbdecae85bef5287df753ea0775e89a061c581e67695d26ec3aa7e2e05bff4fcacc1063a3091d9345fae2a55d8035702794384fe524fba98e81333681b4ed9ac848dae128c453d10045dbb9da90f84275aa1516654a84851b728c99a6d003cc2db1c059ca7a129d36c1ba8602f531b57ffaaf9cd7dc8dd56ef6dda1f583b0828f995162fb73fe3cb8f7de53aed5b789a5b570d563bb4c2b91cef3b0126869eb9d9acbbef8f38e519b87785c49706b0e88799004cca1afdfcf655e989c6ca92599888d1bf9c7ff4f149a61c79e101c6ce625e65e2c2cb13464313aacbaaabd06fb6cd678b96f68306a54e5c2961ffe02382102715af7d",
            "730ebd51ff60434abb1a2533915601d6b26eb041c0f16a57ef4f80ef7543f25c265337cb95fde30f99888a6abe1cd463a17e41cbe16b9279f955332839fd76d49fb8ce8d668b54c50611b23cbc270de114a5a62cbb263242a47fb16b53660376056ad5ed35ab04fe66b4d85f272d4c56a04bbc82dc9673b9a0ba7d70a4db5449997ccc16ef90952bc43eb4b0d34e93d466df1cb7434c0303014d9efa3ebfc3931940c162eeedd6dba088b7154b16103c5f2c4d81e6ef98d2712e006538c0dccc0b2337ba45dea2c4eefa445d2bf19651bce5c5a1d7343cd92bc54a7d52c96a93abd7a3372a224eb68a095fce4549276aeccc2312bec2095899b55d2673f35f73",
            "216c821e2f1fee7f24afa41941f4df27d5bdf8ae5c753ceb7f110d89504dfda27ab660c96"
        );

        // act
        const isValid: boolean = proof.verify(cipher.c, cipher.d, publicKey, p, g);

        // assert
        expect(isValid).toStrictEqual(true);
    });

    it("should classify proof as invalid", () => {
        const p: bigInt.BigInteger = BigNumberHelper.fromHexString("ffffffffffffffffc90fdaa22168c234c4c6628b80dc1cd129024e088a67cc74020bbea63b139b22514a08798e3404ddef9519b3cd3a431b302b0a6df25f14374fe1356d6d51c245e485b576625e7ec6f44c42e9a637ed6b0bff5cb6f406b7edee386bfb5a899fa5ae9f24117c4b1fe649286651ece45b3dc2007cb8a163bf0598da48361c55d39a69163fa8fd24cf5f83655d23dca3ad961c62f356208552bb9ed529077096966d670c354e4abc9804f1746c08ca18217c32905e462e36ce3be39e772c180e86039b2783a2ec07a28fb5c55df06f4c52c9de2bcbf6955817183995497cea956ae515d2261898fa051015728e5a8aacaa68ffffffffffffffff");
        const g: bigInt.BigInteger = BigNumberHelper.fromHexString("2");
        const publicKey: bigInt.BigInteger = BigNumberHelper.fromHexString("505a3e9a453ca5177325acfab3fb1e5e4c6090c7395e995856b599dd6197509873dee31a57eb667a287ea650d4acdd65c15775268134f8e0897aeee3df83e57e27d7d84be71a25e441d4ebcad39684869a4c19abe62fd05c549dbf7de7f2a920addbb4bb4143badc29fe71f0724aa1f6a2550324e16bc70fbcbdfc8eb0bce28273fc615ba907e521ad8d8ae57cc4f54d2f84f31e15bc4be53095604b6403febcffd4b79453da2d4aee8f7685728f5472fa33484acb878be6cfe450c7ae44bf600dd8d611d2ce79ae14f040661a8b43e2fd5791e47a9452e0565bf9e3d027df97bf7e3209422014196170252ee55bf349d2230754537ddde0bbd8f446159c3cf6");

        const cipher: Cipher = {
            c: BigNumberHelper.fromHexString("2b66853b6009043222870f2dda9a3d8ae3ecfc9aa5dd8199180f2659495bb2b57f44d8568f4e53df457e6bc1f38922ab52203b7bda92fe1ddf2f08cf4e5f9f4f1b8507dc1da57f6b374eca2cd2e392c6823387b5d4a4ffbe3a9617331a42c3a1bddcae63baee2e533ce5d506ca2a45407cd5bc6561cb7a45332e1695538f05a4503aab7c7f11a510bc59c0f926b12ceadeb6318f4f057eb2dc760aefb3744e48283d286238cce139b7b76d111225f070906c49ce86c557b76a75bd6ec4e9e6ccca49ddb034dac936b94940237795cd03c91cba15fe09221268ee1bdc0b18476018533f88afe6a3a5ca6846b32b4f2e77fa484567c261420a6e44d506ece0340f"),
            d: BigNumberHelper.fromHexString("cfd824346e446c6e738da939ef1dd6beac4ffa3538a2b0b8631d093c356aceed5b89310fb8484630de04a6fdf0feeb990d5eab0f450a195206f9d042dfa59bd84603b6ddb844fd52461f64f7691236db2f3bec7270b0859706323bd0fbde68563ab92c124127398cf3e596b1a6de4fb04a8347ebe5e9df7e875a1275f9ed1031557b7b5b02f6f3708f4012fd36338a9c6538222a27df6d722f30976f7c3e6590ca058ebc68ca9cee25dd8f38bae394e36eecc7393cdfbdbb8f39349a62e0b22647274139f7a0020aa0bc74e65989059a02f233eb0269eb2e8c4224be6b5e2cf61989b59ab4d9da833b5c22b9df18169f084406fcee44ae85b70b14e2cf163c76")
        }

        const proof: ProofOfContainingOne = new ProofOfContainingOne(
            "3be6d1a56940d5892cddcbdecae85bef5287df753ea0775e89a061c581e67695d26ec3aa7e2e05bff4fcacc1063a3091d9345fae2a55d8035702794384fe524fba98e81333681b4ed9ac848dae128c453d10045dbb9da90f84275aa1516654a84851b728c99a6d003cc2db1c059ca7a129d36c1ba8602f531b57ffaaf9cd7dc8dd56ef6dda1f583b0828f995162fb73fe3cb8f7de53aed5b789a5b570d563bb4c2b91cef3b0126869eb9d9acbbef8f38e519b87785c49706b0e88799004cca1afdfcf655e989c6ca92599888d1bf9c7ff4f149a61c79e101c6ce625e65e2c2cb13464313aacbaaabd06fb6cd678b96f68306a54e5c2961ffe02382102715af7d",
            "730ebd51ff60434abb1a2533915601d6b26eb041c0f16a57ef4f80ef7543f25c265337cb95fde30f99888a6abe1cd463a17e41cbe16b9279f955332839fd76d49fb8ce8d668b54c50611b23cbc270de114a5a62cbb263242a47fb16b53660376056ad5ed35ab04fe66b4d85f272d4c56a04bbc82dc9673b9a0ba7d70a4db5449997ccc16ef90952bc43eb4b0d34e93d466df1cb7434c0303014d9efa3ebfc3931940c162eeedd6dba088b7154b16103c5f2c4d81e6ef98d2712e006538c0dccc0b2337ba45dea2c4eefa445d2bf19651bce5c5a1d7343cd92bc54a7d52c96a93abd7a3372a224eb68a095fce4549276aeccc2312bec2095899b55d2673f35f73",
            "216c821e2f1fee7f24afa41941f4df27d5bdf8ae5c753ceb7f110d89504dfda27ab660c96"
        );

        // act
        const isValid: boolean = proof.verify(cipher.c, cipher.d, publicKey, p, g);

        // assert
        expect(isValid).toStrictEqual(false);
    });
})