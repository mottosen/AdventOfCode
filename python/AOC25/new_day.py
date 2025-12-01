import shutil
from pathlib import Path

dir_aoc = Path(".")
dir_template = dir_aoc / ".template"
days = 12

for day in range(1, days + 1):
    dir_day = dir_aoc / f"Day{day}"

    if not dir_day.exists():
        print(f"Setting up Day {day}!")
        shutil.copytree(dir_template, dir_day)
        break
else:
    print("No more days this year!")
