import re

with open("input.txt", "r") as f:
    lines = f.readlines()

elf_max = 0
elf_tmp = 0

for line in lines:
    line = re.match("\d+", line)

    if (line == None):
        if (elf_tmp > elf_max):
            elf_max = elf_tmp
        elf_tmp = 0
        continue

    num = int(line.group())
    elf_tmp += num

if (elf_tmp > elf_max):
    elf_max = elf_tmp

print(elf_max)