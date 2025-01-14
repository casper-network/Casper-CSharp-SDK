﻿using EnvisionStaking.Casper.SDK.Enums;
using EnvisionStaking.Casper.SDK.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnvisionStaking.Casper.SDK.Test
{

    [TestClass]
    public class SigningServiceTest
    {
        string rpcUrl = "http://40.69.22.98:7777/rpc";
        string accountKey = "0202ba37a693fb6494b3c42a65f07a6123dd125d8bf8a16e10ec7b95b826b151230c";

        [TestMethod]
        public void SignApprovalEd25519()
        {
            string accountKey = "0123e52cf5d878e4ba3c388a6e1969a56a5b86d52f3c8fd0dd8463797c90b4dad6";
            string deployHash = "55761719042cf6ffd1b74005cd946a9d70dd363a63a495f5ecaa6d4990a256d5";
            string signature = "01a99e76dd974450b0ecc74c3674e4dd756a08aa983c2478f04b628fb34e4654095f9c10530b99949970a92407946c4e5ee660dbddd9f6b2edf114706763cf340f";

            CasperClient casperClient = new CasperClient(rpcUrl);

            var keyPair = casperClient.SigningService.GetKeyPairFromFile(@"keys\Ed25519_Test_public_key.pem", @"keys\Ed25519_Test_secret_key.pem", SignAlgorithmEnum.ed25519);

            var approvalResult = casperClient.DeployService.SignApproval(accountKey,deployHash, keyPair);          

            Assert.AreEqual(approvalResult.signer,accountKey);
            Assert.AreEqual(approvalResult.signature.Substring(0, 2), "01");
            Assert.AreEqual(approvalResult.signature, signature);
        }

        [TestMethod]
        public void VerifySignatureByGeneratedKeyPairEd25519()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GenerateKeyPair(SignAlgorithmEnum.ed25519);

            var messageToSign = Encoding.UTF8.GetBytes("Test Message");

            var signedMessage = casperClient.SigningService.GetSignatureEd25519(keyPair.Private, messageToSign);

            var signatureIsVerified = casperClient.SigningService.VerifySignatureEd25519(keyPair.Public, messageToSign, signedMessage);

            Assert.IsTrue(signatureIsVerified);
        }

        [TestMethod]
        public void VerifySignatureByGeneratedKeyPairChangeMessageEd25519()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GenerateKeyPair(SignAlgorithmEnum.ed25519);

            var messageToSign = Encoding.UTF8.GetBytes("Test Message");
            var messageToSignChanged = Encoding.UTF8.GetBytes("Test Message Changed");

            var signedMessage = casperClient.SigningService.GetSignatureEd25519(keyPair.Private, messageToSign);

            var signatureIsVerified = casperClient.SigningService.VerifySignatureEd25519(keyPair.Public, messageToSignChanged, signedMessage);

            Assert.IsFalse(signatureIsVerified);
        }

        [TestMethod]
        public void GetKeyPairEd25519()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);

            FileStream publicKeyStream = File.OpenRead(@"keys\Ed25519_Test_public_key.pem");
            FileStream privateKeyStream = File.OpenRead(@"keys\Ed25519_Test_secret_key.pem");

            var keyPair = casperClient.SigningService.GetKeyPair(publicKeyStream, privateKeyStream, SignAlgorithmEnum.ed25519);

            Assert.IsNotNull(keyPair);
        }

        [TestMethod]
        public void GetKeyPairSec256k1()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);

            FileStream publicKeyStream = File.OpenRead(@"keys\Secp256k1_Test_public_key.pem");
            FileStream privateKeyStream = File.OpenRead(@"keys\Secp256k1_Test_secret_key.pem");

            var keyPair = casperClient.SigningService.GetKeyPair(publicKeyStream, privateKeyStream, SignAlgorithmEnum.secp256k1);

            Assert.IsNotNull(keyPair);
        }


        [TestMethod]
        public void VerifySignatureByKeyPairFromFileEd25519()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GetKeyPairFromFile(@"keys\Ed25519_Test_public_key.pem", @"keys\Ed25519_Test_secret_key.pem", SignAlgorithmEnum.ed25519);

            var messageToSign = Encoding.UTF8.GetBytes("Test Message");

            var signedMessage = casperClient.SigningService.GetSignatureEd25519(keyPair.Private, messageToSign);

            var signatureIsVerified = casperClient.SigningService.VerifySignatureEd25519(keyPair.Public, messageToSign, signedMessage);

            Assert.IsTrue(signatureIsVerified);
        }

        [TestMethod]
        public void VerifySignatureByKeyPairFromFileChangeMessageEd25519()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GetKeyPairFromFile(@"keys\Ed25519_Test_public_key.pem", @"keys\Ed25519_Test_secret_key.pem", SignAlgorithmEnum.ed25519);

            var messageToSign = Encoding.UTF8.GetBytes("Test Message");
            var messageToSignChanged = Encoding.UTF8.GetBytes("Test Message Changed");

            var signedMessage = casperClient.SigningService.GetSignatureEd25519(keyPair.Private, messageToSign);

            var signatureIsVerified = casperClient.SigningService.VerifySignatureEd25519(keyPair.Public, messageToSignChanged, signedMessage);

            Assert.IsFalse(signatureIsVerified);
        }

        [TestMethod]
        public void SignApprovalSecp256k1()
        {
            string accountKey = "0203a9cd2472eeedb7081dd87ecae04d8fe1cedbf5e6a9fcb158ad966d94c63d2c6d";
            string deployHash = "b96bc0f44dd79c6793d16c52e53760004367c8400de0eb17e46edda75289a856";
            
            CasperClient casperClient = new CasperClient(rpcUrl);

            var keyPair = casperClient.SigningService.GetKeyPairFromFile(@"keys\Secp256k1_Test_public_key.pem", @"keys\Secp256k1_Test_secret_key.pem", SignAlgorithmEnum.secp256k1);

            var approvalResult = casperClient.DeployService.SignApproval(accountKey, deployHash, keyPair);

            var signatureIsVerified = casperClient.SigningService.VerifySignatureSecp256k1(keyPair.Public, ByteUtil.HexToByteArray(deployHash), ByteUtil.HexToByteArray(approvalResult.signature.Substring(2)));

            Assert.AreEqual(approvalResult.signature.Substring(0,2), "02");
            Assert.AreEqual(approvalResult.signer, accountKey);
            Assert.IsTrue(signatureIsVerified);
        }

        [TestMethod]
        public void VerifySignatureByGeneratedKeyPairSecp256k1()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GenerateKeyPair(SignAlgorithmEnum.secp256k1);

            var messageToSign = Encoding.UTF8.GetBytes("b96bc0f44dd79c6793d16c52e53760004367c8400de0eb17e46edda75289a856");

            var signedMessage = casperClient.SigningService.GetSignatureSecp256k1(keyPair.Private, messageToSign);

            var signatureIsVerified = casperClient.SigningService.VerifySignatureSecp256k1(keyPair.Public, messageToSign, signedMessage);

            Assert.IsTrue(signatureIsVerified);
        }

        [TestMethod]
        public void VerifySignatureByGeneratedKeyPairChangeMessageSecp256k1()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GenerateKeyPair(SignAlgorithmEnum.secp256k1);

            var messageToSign = Encoding.UTF8.GetBytes("Test Message");
            var messageToSignChanged = Encoding.UTF8.GetBytes("Test Message Changed");

            var signedMessage = casperClient.SigningService.GetSignatureSecp256k1(keyPair.Private, messageToSign);

            var signatureIsVerified = casperClient.SigningService.VerifySignatureSecp256k1(keyPair.Public, messageToSignChanged, signedMessage);

            Assert.IsFalse(signatureIsVerified);
        }

        [TestMethod]
        public void VerifySignatureByKeyPairFromFileSecp256k1()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GetKeyPairFromFile(@"keys\Secp256k1_Test_public_key.pem", @"keys\Secp256k1_Test_secret_key.pem", SignAlgorithmEnum.secp256k1);

            var messageToSign = Encoding.UTF8.GetBytes("Test Message");

            var signedMessage = casperClient.SigningService.GetSignatureSecp256k1(keyPair.Private, messageToSign);

            var signatureIsVerified = casperClient.SigningService.VerifySignatureSecp256k1(keyPair.Public, messageToSign, signedMessage);

            Assert.IsTrue(signatureIsVerified);
        }

        [TestMethod]
        public void VerifySignatureByKeyPairFromFileChangeMessageSecp256k1()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GetKeyPairFromFile(@"keys\Secp256k1_Test_public_key.pem", @"keys\Secp256k1_Test_secret_key.pem", SignAlgorithmEnum.secp256k1);

            var messageToSign = Encoding.UTF8.GetBytes("Test Message");
            var messageToSignChanged = Encoding.UTF8.GetBytes("Test Message Changed");

            var signedMessage = casperClient.SigningService.GetSignatureSecp256k1(keyPair.Private, messageToSign);

            var signatureIsVerified = casperClient.SigningService.VerifySignatureSecp256k1(keyPair.Public, messageToSignChanged, signedMessage);

            Assert.IsFalse(signatureIsVerified);
        }

        [TestMethod]
        public void VerifySignatureByKeyPairFromFileFromDeployHashEd25519()
        {
            // Deploy hash in bytes
            byte[] message = {
                (byte) 153, (byte) 144, (byte) 19, (byte) 83, (byte) 219, (byte) 161, (byte) 143, (byte) 137, (byte) 59,
                (byte) 67, (byte) 187, (byte) 238, (byte) 65, (byte) 111, (byte) 80, (byte) 243, (byte) 142, (byte) 77,
                (byte) 113, (byte) 46, (byte) 2, (byte) 166, (byte) 121, (byte) 118, (byte) 34, (byte) 205, (byte) 123,
                (byte) 14, (byte) 215, (byte) 85, (byte) 234, (byte) 161
        };

            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GetKeyPairFromFile(@"keys\Ed25519_Test_public_key.pem", @"keys\Ed25519_Test_secret_key.pem", SignAlgorithmEnum.ed25519);

            var signedMessage = casperClient.SigningService.GetSignatureEd25519(keyPair.Private, message);

            bool signatureIsVerified = casperClient.SigningService.VerifySignatureEd25519(keyPair.Public, message, signedMessage);

            Assert.AreEqual(signedMessage.Length, 64);
            Assert.IsTrue(signatureIsVerified);
        }

        [TestMethod]
        public void ConvertPrivateKeyToPemEd25519()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GetKeyPairFromFile(@"keys\Ed25519_Test_public_key.pem", @"keys\Ed25519_Test_secret_key.pem", SignAlgorithmEnum.ed25519);

            var privateKeyPem = casperClient.SigningService.ConvertPrivateKeyToPem(keyPair.Private);

            Assert.IsTrue(!string.IsNullOrEmpty(privateKeyPem));
        }

        [TestMethod]
        public void ConvertPublicKeyToPemEd25519()
        {
            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GetKeyPairFromFile(@"keys\Ed25519_Test_public_key.pem", @"keys\Ed25519_Test_secret_key.pem", SignAlgorithmEnum.ed25519);

            var publicKeyPem = casperClient.SigningService.ConvertPublicKeyToPem(keyPair.Public);

            Assert.IsTrue(!string.IsNullOrEmpty(publicKeyPem));
        }

        [TestMethod]
        public void GetSignatureEd25519()
        {
            string account = "0123e52cf5d878e4ba3c388a6e1969a56a5b86d52f3c8fd0dd8463797c90b4dad6";
            string deployHash = "55761719042cf6ffd1b74005cd946a9d70dd363a63a495f5ecaa6d4990a256d5";
            string signedValue = "01a99e76dd974450b0ecc74c3674e4dd756a08aa983c2478f04b628fb34e4654095f9c10530b99949970a92407946c4e5ee660dbddd9f6b2edf114706763cf340f";

            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GetKeyPairFromFile(@"keys\Ed25519_Test_public_key.pem", @"keys\Ed25519_Test_secret_key.pem", SignAlgorithmEnum.ed25519);

            var signedValueResultBytes = casperClient.SigningService.GetSignatureEd25519(keyPair.Private, ByteUtil.HexToByteArray(deployHash));
            var signedValueResultHex = ByteUtil.ByteArrayToHex(signedValueResultBytes);
            var result = account.Substring(0, 2) + signedValueResultHex;

            Assert.AreEqual(result, signedValue);
        }
        
        [TestMethod]
        public void GetSignatureSecp256k1()
        {
            string account = "0203a9cd2472eeedb7081dd87ecae04d8fe1cedbf5e6a9fcb158ad966d94c63d2c6d";
            string deployHash = "207ecc7c47ebba4d71e9911702fa14d225ec78aab255ac82a59666c4b352bd81";

            CasperClient casperClient = new CasperClient(rpcUrl);
            var keyPair = casperClient.SigningService.GetKeyPairFromFile(@"keys\Secp256k1_Test_public_key.pem", @"keys\Secp256k1_Test_secret_key.pem", SignAlgorithmEnum.secp256k1);

            var signedValueResultBytes = casperClient.SigningService.GetSignatureSecp256k1(keyPair.Private, ByteUtil.HexToByteArray(deployHash));
            var signedValueResultHex = ByteUtil.ByteArrayToHex(signedValueResultBytes);
            var result = account.Substring(0, 2) + signedValueResultHex;

            Assert.IsTrue(result.Length == 130);
        }
    }
}

