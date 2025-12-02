import itertools
import multiprocessing as mp
import re
from functools import partial
from pathlib import Path

import Parts.Util.handlers as handlers


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            matches = re.findall(r"(\d+)-(\d+)", f.readline())

        self.worker_pool = mp.Pool()
        self.id_ranges = [range(int(match[0]), int(match[1]) + 1) for match in matches]

    def get_day_solution(self):
        return str(
            sum(  # summing invalid ids
                itertools.chain(  # chaining results
                    *self.worker_pool.map(  # handle ranges in parallel
                        partial(handlers.handle_range, handlers.handle_id_2),
                        self.id_ranges,
                    )
                )
            )
        )
