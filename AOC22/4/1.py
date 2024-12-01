import re

def fully_enclosed(r1, r2):
    return ((r1[0] <= r2[0] and r1[1] >= r2[1]) or (r1[0] >= r2[0] and r1[1] <= r2[1]))

with open("input.txt", "r") as f:
    lines = f.readlines()

total_count = 0

for line in lines:
    line = re.match("^(\d+)-(\d+),(\d+)-(\d+)$", line)

    if line != None:
        ll, lh = int(line.group(1)), int(line.group(2))
        rl, rh = int(line.group(3)), int(line.group(4))

        foo = fully_enclosed((ll, lh), (rl, rh))
        if foo:
            total_count += 1

print(total_count)