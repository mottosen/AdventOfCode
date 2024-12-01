import re
import bisect

with open("input.txt", "r") as f:
    lines = f.readlines()

elves = list()
elf_trd = -3
elf_tmp = 0

for line in lines:  
    line = re.match("\d+", line)

    if (line == None):
        if (elf_trd < 0):
            bisect.insort(elves, elf_tmp)
            elf_trd += 1
        elif (elf_tmp > elf_trd):
            bisect.insort(elves, elf_tmp)
            elves = elves[1:]
            elf_trd = elves[0]
        elf_tmp = 0
        continue

    num = int(line.group())
    elf_tmp += num

if (elf_tmp > elf_trd):
    bisect.insort(elves, elf_tmp)
    elves = elves[1:]

print(sum(elves))