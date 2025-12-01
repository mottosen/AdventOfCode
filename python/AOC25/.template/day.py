import os
import sys
import time
from pathlib import Path

from Parts.Part1 import Part1
from Parts.Part2 import Part2


def main():
    """
    Get solutions for today, just supply the input file, like:

        python day.py <input_file>

    """
    try:
        input_file: Path = Path(sys.argv[1])

        # -------- Part1 --------
        print("\nComputing result for Part 1...\n")
        part1: Part1 = Part1(input_file)

        time_start: float = time.perf_counter()
        solution1: str = part1.get_day_solution()
        time_stop: float = time.perf_counter()

        print(f"Answer: {solution1}\nFound in: {time_stop - time_start:0.4f} seconds\n")

        # -------- Part2 --------
        print("\nComputing result for Part 2...\n")
        part2: Part2 = Part2(input_file)

        time_start: float = time.perf_counter()
        solution1: str = part2.get_day_solution()
        time_stop: float = time.perf_counter()

        print(f"Answer: {solution1}\nFound in: {time_stop - time_start:0.4f} seconds\n")

    except Exception as e:
        print(f"Did you look at this usage?\n{main.__doc__}\n\nGot Error:\n{e}")
        exit(1)

    else:
        exit(0)


if __name__ == "__main__":
    os.system("clear")
    main()
