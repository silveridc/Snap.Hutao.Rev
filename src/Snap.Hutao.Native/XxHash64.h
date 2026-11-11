#pragma once

#include <cstdint>
#include <cstring>

namespace
{
    // 与 System.IO.Hashing.XxHash64.HashToUInt64 逐位一致，协议校验依赖这一点。
    class XxHash64
    {
    private:
        static constexpr uint64_t kPrime1 = 11400714785074694791ULL;
        static constexpr uint64_t kPrime2 = 14029467366897019727ULL;
        static constexpr uint64_t kPrime3 = 1609587929392839161ULL;
        static constexpr uint64_t kPrime4 = 9650029242287828579ULL;
        static constexpr uint64_t kPrime5 = 2870177450012600261ULL;

        [[nodiscard]] static uint64_t RotateLeft(uint64_t value, int bits) noexcept
        {
            return (value << bits) | (value >> (64 - bits));
        }

        [[nodiscard]] static uint64_t Read64(const uint8_t* p) noexcept
        {
            uint64_t value;
            memcpy(&value, p, sizeof(value));
            return value;
        }

        [[nodiscard]] static uint32_t Read32(const uint8_t* p) noexcept
        {
            uint32_t value;
            memcpy(&value, p, sizeof(value));
            return value;
        }

        [[nodiscard]] static uint64_t Round(uint64_t acc, uint64_t input) noexcept
        {
            acc += input * kPrime2;
            acc = RotateLeft(acc, 31);
            acc *= kPrime1;
            return acc;
        }

        [[nodiscard]] static uint64_t MergeRound(uint64_t acc, uint64_t value) noexcept
        {
            value = Round(0, value);
            acc ^= value;
            return acc * kPrime1 + kPrime4;
        }

    public:
        [[nodiscard]] static uint64_t Hash(const void* data, size_t length) noexcept
        {
            const uint8_t* p = static_cast<const uint8_t*>(data);
            const uint8_t* end = p + length;
            uint64_t hash;

            if (length >= 32)
            {
                const uint8_t* limit = end - 32;
                uint64_t v1 = kPrime1 + kPrime2;
                uint64_t v2 = kPrime2;
                uint64_t v3 = 0;
                uint64_t v4 = 0 - kPrime1;

                do
                {
                    v1 = Round(v1, Read64(p));
                    p += 8;
                    v2 = Round(v2, Read64(p));
                    p += 8;
                    v3 = Round(v3, Read64(p));
                    p += 8;
                    v4 = Round(v4, Read64(p));
                    p += 8;
                }
                while (p <= limit);

                hash = RotateLeft(v1, 1) + RotateLeft(v2, 7) + RotateLeft(v3, 12) + RotateLeft(v4, 18);
                hash = MergeRound(hash, v1);
                hash = MergeRound(hash, v2);
                hash = MergeRound(hash, v3);
                hash = MergeRound(hash, v4);
            }
            else
            {
                hash = kPrime5;
            }

            hash += length;

            while (p + 8 <= end)
            {
                uint64_t k1 = Round(0, Read64(p));
                hash ^= k1;
                hash = RotateLeft(hash, 27) * kPrime1 + kPrime4;
                p += 8;
            }

            if (p + 4 <= end)
            {
                hash ^= static_cast<uint64_t>(Read32(p)) * kPrime1;
                hash = RotateLeft(hash, 23) * kPrime2 + kPrime3;
                p += 4;
            }

            while (p < end)
            {
                hash ^= static_cast<uint64_t>(*p) * kPrime5;
                hash = RotateLeft(hash, 11) * kPrime1;
                ++p;
            }

            hash ^= hash >> 33;
            hash *= kPrime2;
            hash ^= hash >> 29;
            hash *= kPrime3;
            hash ^= hash >> 32;
            return hash;
        }
    };
}
