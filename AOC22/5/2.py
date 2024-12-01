import re
from math import floor

def get_priority(c):
    val = ord(c)

    if re.match("^[a-z]$", c) != None:
        return val - 97
    elif re.match("^[A-Z]$", c) != None:
        return val - 65 + 26
    else:
        return -1

with open("input.txt", "r") as f:
    lines = f.readlines()

total_priority = 0
item_log = [0 for i in range(52)]

for num, line in enumerate(lines):
    elf_in_group = num % 3

    for i, c in enumerate(line):
        p = get_priority(c)
        if p < 0: continue

        if item_log[p] == elf_in_group:
            item_log[p] += 1

    if elf_in_group == 2:
        for i, item in enumerate(item_log):
            if item == 3:
                total_priority += i + 1
            item_log[i] = 0

print(total_priority)