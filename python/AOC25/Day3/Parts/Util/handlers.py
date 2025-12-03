import re


def handle_line(batteries, line):
    all_digits = list(map(int, re.findall(r"\d", line)))
    to_remove = len(all_digits) - batteries

    # stores highest digits found, in reverse order
    stack = []

    # one pass over all digits
    for digit in all_digits:
        # current digit has higher priority over lower digits on the stack
        while stack and to_remove > 0 and stack[0] < digit:
            stack.pop(0)
            to_remove -= 1

        # push current digit when lower priorities are gone
        stack.insert(0, digit)

    # get 12 batteries
    high_digits = stack[-batteries:]

    # convert stack into one multi-digit number
    res = 0
    for i, digit in enumerate(high_digits):
        res += digit * (10**i)

    return res
