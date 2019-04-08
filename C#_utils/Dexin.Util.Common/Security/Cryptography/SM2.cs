using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;

namespace Dexin.Util.Common.Security.Cryptography
{
    public class SM2
    {
        public static SM2 Instance
        {
            get
            {
                return new SM2(false);
            }

        }
        public static SM2 InstanceTest
        {
            get
            {
                return new SM2(true);
            }

        }
        public bool sm2Test = false;

        public string[] ecc_param = sm2_test_param;

        public readonly BigInteger ecc_p;
        public readonly BigInteger ecc_a;
        public readonly BigInteger ecc_b;
        public readonly BigInteger ecc_n;
        public readonly BigInteger ecc_gx;
        public readonly BigInteger ecc_gy;

        public readonly ECCurve ecc_curve;
        public readonly ECPoint ecc_point_g;

        public readonly ECDomainParameters ecc_bc_spec;

        public readonly ECKeyPairGenerator ecc_key_pair_generator;

        private SM2(bool sm2Test)
        {
            this.sm2Test = sm2Test;

            if (sm2Test)
                ecc_param = sm2_test_param;
            else
                ecc_param = sm2_param;

            ECFieldElement ecc_gx_fieldelement;
            ECFieldElement ecc_gy_fieldelement;

            ecc_p = new BigInteger(ecc_param[0], 16);
            ecc_a = new BigInteger(ecc_param[1], 16);
            ecc_b = new BigInteger(ecc_param[2], 16);
            ecc_n = new BigInteger(ecc_param[3], 16);
            ecc_gx = new BigInteger(ecc_param[4], 16);
            ecc_gy = new BigInteger(ecc_param[5], 16);


            ecc_gx_fieldelement = new FpFieldElement(ecc_p, ecc_gx);
            ecc_gy_fieldelement = new FpFieldElement(ecc_p, ecc_gy);

            ecc_curve = new FpCurve(ecc_p, ecc_a, ecc_b);
            ecc_point_g = new FpPoint(ecc_curve, ecc_gx_fieldelement, ecc_gy_fieldelement);

            ecc_bc_spec = new ECDomainParameters(ecc_curve, ecc_point_g, ecc_n);

            ECKeyGenerationParameters ecc_ecgenparam;
            ecc_ecgenparam = new ECKeyGenerationParameters(ecc_bc_spec, new SecureRandom());

            ecc_key_pair_generator = new ECKeyPairGenerator();
            ecc_key_pair_generator.Init(ecc_ecgenparam);
        }

        public virtual byte[] Sm2GetZ(byte[] userId, ECPoint userKey)
        {
            SM3Digest sm3 = new SM3Digest();
            byte[] p;
            // userId length
            int len = userId.Length * 8;
            sm3.Update((byte)(len >> 8 & 0x00ff));
            sm3.Update((byte)(len & 0x00ff));

            // userId
            sm3.BlockUpdate(userId, 0, userId.Length);

            // a,b
            p = ecc_a.ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);
            p = ecc_b.ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);
            // gx,gy
            p = ecc_gx.ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);
            p = ecc_gy.ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);

            // x,y
            p = userKey.X.ToBigInteger().ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);
            p = userKey.Y.ToBigInteger().ToByteArray();
            sm3.BlockUpdate(p, 0, p.Length);

            // Z
            byte[] md = new byte[sm3.GetDigestSize()];
            sm3.DoFinal(md, 0);

            return md;
        }

        public virtual void Sm2Sign(byte[] md, BigInteger userD, SM2Result sm2Ret)
        {
            // e
            BigInteger e = new BigInteger(1, md);
            // k
            BigInteger k = null;
            ECPoint kp = null;
            BigInteger r = null;
            BigInteger s = null;

            do
            {
                do
                {
                    if (!sm2Test)
                    {
                        AsymmetricCipherKeyPair keypair = ecc_key_pair_generator.GenerateKeyPair();
                        ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)keypair.Private;
                        ECPublicKeyParameters ecpub = (ECPublicKeyParameters)keypair.Public;
                        k = ecpriv.D;
                        kp = ecpub.Q;
                    }
                    else
                    {
                        string kS = "6CB28D99385C175C94F94E934817663FC176D925DD72B727260DBAAE1FB2F96F";
                        k = new BigInteger(kS, 16);
                        kp = ecc_point_g.Multiply(k);
                    }

                    // r
                    r = e.Add(kp.X.ToBigInteger());
                    r = r.Mod(ecc_n);
                }
                while (r.Equals(BigInteger.Zero) || r.Add(k).Equals(ecc_n));

                // (1 + dA)~-1
                BigInteger da_1 = userD.Add(BigInteger.One);
                da_1 = da_1.ModInverse(ecc_n);
                // s
                s = r.Multiply(userD);
                s = k.Subtract(s).Mod(ecc_n);
                s = da_1.Multiply(s).Mod(ecc_n);
            }
            while (s.Equals(BigInteger.Zero));

            sm2Ret.r = r;
            sm2Ret.s = s;
        }

        public virtual void Sm2Verify(byte[] md, ECPoint userKey, BigInteger r, BigInteger s, SM2Result sm2Ret)
        {
            sm2Ret.R = null;

            // e_
            BigInteger e = new BigInteger(1, md);
            // t
            BigInteger t = r.Add(s).Mod(ecc_n);

            if (t.Equals(BigInteger.Zero))
                return;

            // x1y1
            ECPoint x1y1 = ecc_point_g.Multiply(sm2Ret.s);
            x1y1 = x1y1.Add(userKey.Multiply(t));

            // R
            sm2Ret.R = e.Add(x1y1.X.ToBigInteger()).Mod(ecc_n);
        }


        /// <summary>
        /// 获取公钥
        /// </summary>
        /// <param name="x">公钥x</param>
        /// <param name="y">公钥y</param>
        /// <param name="radix">格式</param>
        /// <returns></returns>
        public ECPoint CreatePublicKey(string x, string y, int radix)
        {
            BigInteger biX = new BigInteger(x, radix);
            BigInteger biY = new BigInteger(y, radix);
            //ECFieldElement fx = new FpFieldElement(this.ecc_p, biX);
            //ECFieldElement fy = new FpFieldElement(this.ecc_p, biY);
            //ECPoint point = new FpPoint(this.ecc_curve, fx, fy);

            ECPoint point = this.ecc_curve.CreatePoint(biX, biY);

            return point;
        }



        public class SM2Result
        {
            public SM2Result()
            {
            }


            // 签名、验签
            public BigInteger r;
            public BigInteger s;
            public BigInteger R;

            // 密钥交换
            public byte[] sa;
            public byte[] sb;
            public byte[] s1;
            public byte[] s2;

            public ECPoint keyra;
            public ECPoint keyrb;
        }

        public static readonly string[] sm2_test_param = {
			"8542D69E4C044F18E8B92435BF6FF7DE457283915C45517D722EDB8B08F1DFC3",// p,0
			"787968B4FA32C3FD2417842E73BBFEFF2F3C848B6831D7E0EC65228B3937E498",// a,1
			"63E4C6D3B23B0C849CF84241484BFE48F61D59A5B16BA06E6E12D1DA27C5249A",// b,2
			"8542D69E4C044F18E8B92435BF6FF7DD297720630485628D5AE74EE7C32E79B7",// n,3
			"421DEBD61B62EAB6746434EBC3CC315E32220B3BADD50BDC4C4E6C147FEDD43D",// gx,4
			"0680512BCBB42C07D47349D2153B70C4E5D7FDFCBFA36EA1A85841B9E46E09A2" // gy,5
	    };

        public static readonly string[] sm2_param = {
			"FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFF",// p,0
			"FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFC",// a,1
			"28E9FA9E9D9F5E344D5A9E4BCF6509A7F39789F515AB8F92DDBCBD414D940E93",// b,2
			"FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFF7203DF6B21C6052B53BBF40939D54123",// n,3
			"32C4AE2C1F1981195F9904466A39C9948FE30BBFF2660BE1715A4589334C74C7",// gx,4
			"BC3736A2F4F6779C59BDCEE36B692153D0A9877CC62A474002DF32E52139F0A0" // gy,5
	    };


        public class KeyExchange
        {
            private SM2 sm2;

            private BigInteger userD;
            private ECPoint userKey;

            private BigInteger rD;
            private ECPoint rKey;

            internal BigInteger _2w;
            internal BigInteger _2w_1;

            private int ct = 1;
            private SM3Digest sm3keybase = null;
            private int keyOff = 0;
            private byte[] key = null;

            public KeyExchange()
            {
            }


            public virtual void Init(SM2 sm2, BigInteger userD, ECPoint userKey)
            {
                this.sm2 = sm2;
                this.userD = userD;
                this.userKey = userKey;

                AsymmetricCipherKeyPair keypair = null;
                keypair = sm2.ecc_key_pair_generator.GenerateKeyPair();
                ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)keypair.Private;
                ECPublicKeyParameters ecpub = (ECPublicKeyParameters)keypair.Public;
                rD = ecpriv.D;
                rKey = ecpub.Q;

                int w = sm2.ecc_n.BitLength;
                w = w / 2 - 1;
                _2w = BigInteger.One.ShiftLeft(w);
                _2w_1 = _2w.Subtract(BigInteger.One);
            }

            public virtual void Init_test_a(SM2 sm2, BigInteger userD, ECPoint userKey)
            {
                Init(sm2, userD, userKey);
                rD = new BigInteger("83A2C9C8B96E5AF70BD480B472409A9A327257F1EBB73F5B073354B248668563", 16);
                rKey = sm2.ecc_point_g.Multiply(rD);
                // System.out.println("A用户 R (D,x,y)= \n" + rD.ToString(16));
                // System.out.println(rKey.X.ToBigInteger().ToString(16));
                // System.out.println(rKey.Y.ToBigInteger().ToString(16));
            }

            public virtual void Init_test_b(SM2 sm2, BigInteger userD, ECPoint userKey)
            {
                Init(sm2, userD, userKey);
                rD = new BigInteger("33FE21940342161C55619C4A0C060293D543C80AF19748CE176D83477DE71C80", 16);
                rKey = sm2.ecc_point_g.Multiply(rD);
                // System.out.println("B用户 R (D,x,y)= \n" + rD.ToString(16));
                // System.out.println(rKey.X.ToBigInteger().ToString(16));
                // System.out.println(rKey.Y.ToBigInteger().ToString(16));
            }

            public virtual void DoA_1_3(SM2Result sm2Ret)
            {
                sm2Ret.keyra = rKey;
            }

            public virtual void DoB_1_10(byte[] ida, byte[] idb, ECPoint keyusera, ECPoint keyra, SM2Result sm2Ret)
            {
                sm2Ret.keyrb = rKey;
                sm2Ret.sb = null;
                sm2Ret.s2 = null;

                byte[] za = null, zb = null;
                if (ida != null)
                    za = sm2.Sm2GetZ(ida, keyusera);
                if (idb != null)
                    zb = sm2.Sm2GetZ(idb, this.userKey);

                BigInteger x2_ = _2w.Add(this.rKey.X.ToBigInteger().And(_2w_1));
                // System.out.println("x2_ = \n" + x2_.ToString(16));

                BigInteger tb = this.userD.Add(this.rD.Multiply(x2_)).Mod(sm2.ecc_n);
                // System.out.println("tb = \n" + tb.ToString(16));

                BigInteger x1_ = _2w.Add(keyra.X.ToBigInteger().And(_2w_1));
                // System.out.println("x1_ = \n" + x1_.ToString(16));

                ECPoint pa0 = keyra.Multiply(x1_);
                // System.out.println("pa0 (x,y) = ");
                // System.out.println(pa0.X.ToBigInteger().ToString(16));
                // System.out.println(pa0.Y.ToBigInteger().ToString(16));

                ECPoint pa1 = keyusera.Add(pa0);
                // System.out.println("pa1 (x,y) = ");
                // System.out.println(pa1.X.ToBigInteger().ToString(16));
                // System.out.println(pa1.Y.ToBigInteger().ToString(16));

                ECPoint pv = pa1.Multiply(tb.Multiply(sm2.ecc_bc_spec.H));
                // System.out.println("pv (x,y) = ");
                // System.out.println(pv.X.ToBigInteger().ToString(16));
                // System.out.println(pv.Y.ToBigInteger().ToString(16));
                if (pv.IsInfinity)
                    return;

                // key base
                byte[] p;
                sm3keybase = new SM3Digest();
                p = pv.X.ToBigInteger().ToByteArray();
                sm3keybase.BlockUpdate(p, 0, p.Length);
                p = pv.Y.ToBigInteger().ToByteArray();
                sm3keybase.BlockUpdate(p, 0, p.Length);
                p = za;
                sm3keybase.BlockUpdate(p, 0, p.Length);
                p = zb;
                sm3keybase.BlockUpdate(p, 0, p.Length);

                ct = 1;
                key = new byte[32];
                keyOff = 0;
                NextKey();

                // hash & hash
                byte[] mdb = new byte[32];
                byte[] mdsb = new byte[32];
                byte[] mds2 = new byte[32];
                SM3Digest hashb = new SM3Digest();
                p = pv.X.ToBigInteger().ToByteArray();
                hashb.BlockUpdate(p, 0, p.Length);

                if (za != null)
                    hashb.BlockUpdate(za, 0, za.Length);
                if (zb != null)
                    hashb.BlockUpdate(zb, 0, zb.Length);

                p = keyra.X.ToBigInteger().ToByteArray();
                hashb.BlockUpdate(p, 0, p.Length);
                p = keyra.Y.ToBigInteger().ToByteArray();
                hashb.BlockUpdate(p, 0, p.Length);
                p = this.rKey.X.ToBigInteger().ToByteArray();
                hashb.BlockUpdate(p, 0, p.Length);
                p = this.rKey.Y.ToBigInteger().ToByteArray();
                hashb.BlockUpdate(p, 0, p.Length);
                hashb.DoFinal(mdb, 0);
                // String smd = new String(Hex.encode(mdb));
                // System.out.println("hash1 = \n" + smd);

                hashb.Reset();
                hashb.Update((byte)0x02);
                p = pv.Y.ToBigInteger().ToByteArray();
                hashb.BlockUpdate(p, 0, p.Length);
                p = mdb;
                hashb.BlockUpdate(p, 0, p.Length);
                hashb.DoFinal(mdsb, 0);

                // String smdsb = new String(Hex.encode(mdsb));
                // System.out.println("sb = \n" + smdsb);

                hashb.Reset();
                hashb.Update((byte)0x03);
                p = pv.Y.ToBigInteger().ToByteArray();
                hashb.BlockUpdate(p, 0, p.Length);
                p = mdb;
                hashb.BlockUpdate(p, 0, p.Length);
                hashb.DoFinal(mds2, 0);

                // String smds2 = new String(Hex.encode(mds2));
                // System.out.println("s2 = \n" + smds2);

                sm2Ret.s2 = mds2;
                sm2Ret.sb = mdsb;
            }

            public virtual void DoA_4_10(byte[] ida, byte[] idb, ECPoint keyuserb, ECPoint keyrb, SM2Result sm2Ret)
            {
                sm2Ret.keyra = rKey;
                sm2Ret.sa = null;
                sm2Ret.s1 = null;

                byte[] za = null, zb = null;
                if (ida != null)
                    za = sm2.Sm2GetZ(ida, this.userKey);
                if (idb != null)
                    zb = sm2.Sm2GetZ(idb, keyuserb);

                BigInteger x1_a = _2w.Add(rKey.X.ToBigInteger().And(_2w_1));
                // System.out.println("x1_a = \n" + x1_a.ToString(16));

                BigInteger x2_a = _2w.Add(keyrb.X.ToBigInteger().And(_2w_1));
                // System.out.println("x2_a = \n" + x2_a.ToString(16));

                BigInteger ta = userD.Add(rD.Multiply(x1_a)).Mod(sm2.ecc_n);
                // System.out.println("ta = \n" + ta.ToString(16));

                ECPoint pb0 = keyrb.Multiply(x2_a);
                // System.out.println("pb0 (x,y) = " );
                // System.out.println(pb0.X.ToBigInteger().ToString(16));
                // System.out.println(pb0.Y.ToBigInteger().ToString(16));

                ECPoint pb1 = keyuserb.Add(pb0);
                // System.out.println("pb1 (x,y) = " );
                // System.out.println(pb1.X.ToBigInteger().ToString(16));
                // System.out.println(pb1.Y.ToBigInteger().ToString(16));

                ECPoint pu = pb1.Multiply(ta.Multiply(sm2.ecc_bc_spec.H));
                // System.out.println("pu (x,y) = " );
                // System.out.println(pu.X.ToBigInteger().ToString(16));
                // System.out.println(pu.Y.ToBigInteger().ToString(16));
                if (pu.IsInfinity)
                    return;

                byte[] p1;
                SM3Digest sm3keybase1 = new SM3Digest();
                p1 = pu.X.ToBigInteger().ToByteArray();
                sm3keybase1.BlockUpdate(p1, 0, p1.Length);
                p1 = pu.Y.ToBigInteger().ToByteArray();
                sm3keybase1.BlockUpdate(p1, 0, p1.Length);
                p1 = za;
                sm3keybase1.BlockUpdate(p1, 0, p1.Length);
                p1 = zb;
                sm3keybase1.BlockUpdate(p1, 0, p1.Length);

                byte[] p;
                // key base
                sm3keybase = new SM3Digest();
                p = pu.X.ToBigInteger().ToByteArray();
                sm3keybase.BlockUpdate(p, 0, p.Length);
                p = pu.Y.ToBigInteger().ToByteArray();
                sm3keybase.BlockUpdate(p, 0, p.Length);
                p = za;
                sm3keybase.BlockUpdate(p, 0, p.Length);
                p = zb;
                sm3keybase.BlockUpdate(p, 0, p.Length);

                ct = 1;
                key = new byte[32];
                keyOff = 0;
                NextKey();

                // hash & sa & s1
                byte[] mda = new byte[32];
                byte[] mdsa = new byte[32];
                byte[] mds1 = new byte[32];
                SM3Digest hasha = new SM3Digest();
                p = pu.X.ToBigInteger().ToByteArray();
                hasha.BlockUpdate(p, 0, p.Length);

                if (za != null)
                    hasha.BlockUpdate(za, 0, za.Length);
                if (zb != null)
                    hasha.BlockUpdate(zb, 0, zb.Length);

                p = rKey.X.ToBigInteger().ToByteArray();
                hasha.BlockUpdate(p, 0, p.Length);
                p = rKey.Y.ToBigInteger().ToByteArray();
                hasha.BlockUpdate(p, 0, p.Length);
                p = keyrb.X.ToBigInteger().ToByteArray();
                hasha.BlockUpdate(p, 0, p.Length);
                p = keyrb.Y.ToBigInteger().ToByteArray();
                hasha.BlockUpdate(p, 0, p.Length);
                hasha.DoFinal(mda, 0);

                // String smd1 = new String(Hex.encode(mda));
                // System.out.println("hash1 = \n" + smd1);

                // sa
                hasha.Reset();
                hasha.Update((byte)0x03);
                p = pu.Y.ToBigInteger().ToByteArray();
                hasha.BlockUpdate(p, 0, p.Length);
                p = mda;
                hasha.BlockUpdate(p, 0, p.Length);
                hasha.DoFinal(mdsa, 0);

                // String smdsa = new String(Hex.encode(mdsa));
                // System.out.println("sa = \n" + smdsa);

                hasha.Reset();
                hasha.Update((byte)0x02);
                p = pu.Y.ToBigInteger().ToByteArray();
                hasha.BlockUpdate(p, 0, p.Length);
                p = mda;
                hasha.BlockUpdate(p, 0, p.Length);
                hasha.DoFinal(mds1, 0);

                // String smds1 = new String(Hex.encode(mds1));
                // System.out.println("s1 = \n" + smds1);

                sm2Ret.s1 = mds1;
                sm2Ret.sa = mdsa;
            }

            public virtual void GetKey(byte[] keybuf)
            {
                for (int i = 0; i < keybuf.Length; i++)
                {
                    if (keyOff == key.Length)
                        NextKey();

                    keybuf[i] = key[keyOff++];
                }
            }

            private void NextKey()
            {
                SM3Digest sm3keycur = new SM3Digest(sm3keybase);
                sm3keycur.Update((byte)(ct >> 24 & 0x00ff));
                sm3keycur.Update((byte)(ct >> 16 & 0x00ff));
                sm3keycur.Update((byte)(ct >> 8 & 0x00ff));
                sm3keycur.Update((byte)(ct & 0x00ff));
                sm3keycur.DoFinal(key, 0);
                keyOff = 0;
                ct++;
            }
        }

        public class Cipher
        {
            private int ct = 1;

            private ECPoint p2;
            private SM3Digest sm3keybase;
            private SM3Digest sm3c3;

            private byte[] key = new byte[32];
            private byte keyOff = 0;

            public Cipher()
            {
            }

            private void Reset()
            {
                sm3keybase = new SM3Digest();
                sm3c3 = new SM3Digest();

                byte[] p;

                p = p2.X.ToBigInteger().ToByteArray();
                sm3keybase.BlockUpdate(p, 0, p.Length);
                sm3c3.BlockUpdate(p, 0, p.Length);

                p = p2.Y.ToBigInteger().ToByteArray();
                sm3keybase.BlockUpdate(p, 0, p.Length);

                ct = 1;
                NextKey();
            }

            private void NextKey()
            {
                SM3Digest sm3keycur = new SM3Digest(sm3keybase);
                sm3keycur.Update((byte)(ct >> 24 & 0x00ff));
                sm3keycur.Update((byte)(ct >> 16 & 0x00ff));
                sm3keycur.Update((byte)(ct >> 8 & 0x00ff));
                sm3keycur.Update((byte)(ct & 0x00ff));
                sm3keycur.DoFinal(key, 0);
                keyOff = 0;
                ct++;
            }

            public virtual ECPoint Init_enc(SM2 sm2, ECPoint userKey)
            {
                p2 = userKey;
                Reset();

                return userKey;
            }

            public virtual void Encrypt(byte[] data)
            {
                sm3c3.BlockUpdate(data, 0, data.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    if (keyOff == key.Length)
                        NextKey();

                    data[i] ^= key[keyOff++];
                }
            }

            public virtual void Init_dec(SM2 sm2, BigInteger userD)
            {
                p2 = sm2.ecc_point_g.Multiply(userD);
                Reset();
            }

            public virtual void Decrypt(byte[] data)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (keyOff == key.Length)
                        NextKey();

                    data[i] ^= key[keyOff++];
                }
                sm3c3.BlockUpdate(data, 0, data.Length);
            }

            public virtual void Dofinal(byte[] c3)
            {
                byte[] p = p2.Y.ToBigInteger().ToByteArray();
                sm3c3.BlockUpdate(p, 0, p.Length);
                sm3c3.DoFinal(c3, 0);
                Reset();
            }
        }
    }
}
