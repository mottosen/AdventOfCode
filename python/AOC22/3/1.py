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
left = [0 for i in range(52)]
right = [0 for i in range(52)]

for line in lines:
    mid = floor(len(line) / 2)

    for i, c in enumerate(line):
        p = get_priority(c)
        if p < 0: continue

        if i < mid:
            left[p] += 1
        else:
            right[p] += 1

    tmp_priority = -1

    for i in range(52):
        if left[i] > 0 and right[i] > 0:
            tmp_priority = i
        left[i] = 0
        right[i] = 0
    
    total_priority += tmp_priority + 1

print(total_priority)